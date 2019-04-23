using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EthCoffee.api.Models;
using Microsoft.EntityFrameworkCore;

namespace EthCoffee.api.Data
{
    public class TradingRepository : ITradingRepository
    {
        private readonly DataContext _context;

        public TradingRepository(DataContext context)
        {
            _context = context;

        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUserListings(int id)
        {
            var user = await _context.Users
            .Include(a => a.Avatar)
            .Include(l => l.MyListings)
                .ThenInclude(p => p.Photos)
            .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> GetUserDetails(int id)
        {
            var user = await _context.Users
            .Include(a => a.Avatar)
            .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<Listing> GetListing(int id)
        {
            var listing = await _context.Listings
            .Include(p => p.Photos)
            .Include(l => l.User)
            .ThenInclude(u => u.Avatar)
            .FirstOrDefaultAsync(l => l.id == id);
            return listing;
        }

        public async Task<IEnumerable<Listing>> GetListings()
        {
            var listings = await _context.Listings.Include(p => p.Photos).ToListAsync();
            return listings;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ListingPhoto> GetListingPhoto(int id)
        {
            var listingPhoto = await _context.ListingPhotos.FirstOrDefaultAsync(lp => lp.Id == id);
            return listingPhoto;
        }

        public async Task<Avatar> GetAvatar(int id)
        {
            var avatar = await _context.Avatars.FirstOrDefaultAsync(a => a.Id == id);
            return avatar;
        }

        public async Task<ListingPhoto> GetMainListingPhoto(int listingId)
        {
            return await _context.ListingPhotos.Where(l => l.ListingId == listingId)
            .FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}