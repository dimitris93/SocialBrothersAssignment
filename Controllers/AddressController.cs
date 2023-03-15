using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeutrinoAPI;
using NeutrinoAPI.Controllers;
using SocialBrothersAssignment.Data;
using System;
using System.Collections.Generic;
using System.Net;

namespace SocialBrothersAssignment.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly DataContext context;
        private readonly NeutrinoAPIClient client;


        public AddressController(DataContext context)
        {
            this.context = context;
            this.client = new NeutrinoAPIClient("dimitris93", "uUFfga7sTqAST036W429hlPNE63vhHnCBYpzWGHzO6oYuhoj");
        }
        private bool AddressExists(long id)
        {
            return context.Addresses.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<Address>> AddAddress(Address address)
        {
            if(!address.Validate())
            {
                return BadRequest("Address not valid.");
            }

            context.Addresses.Add(address);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Address>>> GetAllAddresses()
        {
            return Ok(await context.Addresses.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult<Address>> GetAddress(long id)
        {
            var address = await context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound("Address not found.");
            }
            return Ok(address);
        }

        [HttpGet("query")]
        public async Task<IActionResult> QueryAddresses(string q, string sortby="id", bool asc=true)
        {
            var addressProperties = typeof(Address).GetProperties();
            var addresses = await context.Addresses.ToListAsync();
            // Convert first letter to upper case
            sortby = sortby[..1].ToUpper() + sortby[1..];
            // Convert to lowercase
            q = q.ToLower();

            // Search
            addresses = addresses.Where(address =>
                                        addressProperties.Any(prop =>
                                        (prop.GetValue(address, null) as string)?.ToLower() == q)).ToList();

            // Sort
            addresses = addresses.OrderBy(i => i.GetType().GetProperty(sortby)?.GetValue(i, null)).ToList();
            if(!asc)
            {
                addresses.Reverse();
            }
            return Ok(addresses);
        }    

        [HttpPut]
        public async Task<IActionResult> PutAddress(long id, Address address)
        {            
            if (id != address.Id)
            {
                return BadRequest("Two different Ids were given.");
            }

            if (!address.Validate())
            {
                return BadRequest("Address not valid.");
            }

            context.Entry(address).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound("Address not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(address);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddress(long id)
        {
            var address = await context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound("Address not found.");
            }

            context.Addresses.Remove(address);
            await context.SaveChangesAsync();

            return Ok("Deleted.");
        }

        [HttpGet("calcDist")]
        public async Task<IActionResult> CalculateDistance(long id, long id2)
        {
            var address = await context.Addresses.FindAsync(id);
            var address2 = await context.Addresses.FindAsync(id2);
            if (address == null)
            {
                return NotFound("The ID of the first address does not exist.");
            }
            if (address2 == null)
            {
                return NotFound("The ID of the second address does not exist.");
            }

            string addressStr = String.Join(" ", new string[] { address.Street, address.HouseNumber, address.ZipCode, address.City, address.Country });
            string address2Str = String.Join(" ", new string[] { address2.Street, address2.HouseNumber, address2.ZipCode, address2.City, address2.Country });

            IGeolocation geolocation = client.Geolocation;

            var result = geolocation.GeocodeAddress(addressStr);
            var result2 = geolocation.GeocodeAddress(address2Str);

            var lat = result.Locations[0].Latitude;
            var lon = result.Locations[0].Longitude;
            var lat2 = result2.Locations[0].Latitude;
            var lon2 = result2.Locations[0].Longitude;

            var latlongs = new List<double?> { lat, lon, lat2, lon2 };

            if (latlongs.All(v => v.HasValue))
            {
                List<double> vars = latlongs.Cast<double>().ToList();
                var dist = GreatCircleDistance(vars[0], vars[1], vars[2], vars[3]);
                return Ok(dist);
            }

            return BadRequest("Something went wrong.");
        }

        // Distance in kilometers
        private static double GreatCircleDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double radius = 6371; // Radius of the Earth in km.
            lat1 = DegreesToRadians(lat1);
            lon1 = DegreesToRadians(lon1);
            lat2 = DegreesToRadians(lat2);
            lon2 = DegreesToRadians(lon2);
            double d_lat = lat2 - lat1;
            double d_lon = lon2 - lon1;
            double h = Math.Sin(d_lat / 2) * Math.Sin(d_lat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(d_lon / 2) * Math.Sin(d_lon / 2);
            return 2 * radius * Math.Asin(Math.Sqrt(h));
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
