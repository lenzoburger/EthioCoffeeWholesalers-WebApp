using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EthCoffee.api.Helpers;
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

        public async Task<IEnumerable<Listing>> GetUserListings(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user.MyListings;
        }

        public async Task<IEnumerable<Listing>> GetWatching(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user.Watchings.Select(w => w.Watching);
        }

        public async Task<IEnumerable<int>> GetWatchingIds(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user.Watchings.Select(w => w.WatchingId);
        }

        public async Task<IEnumerable<User>> GetWatchers(int listingId)
        {
            var listing = await _context.Listings.FirstOrDefaultAsync(l => l.id == listingId);
            return listing.Watchers.Select(w => w.Watcher);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> GetUserDetails(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<Listing> GetListing(int id)
        {
            var listing = await _context.Listings.FirstOrDefaultAsync(l => l.id == id);
            return listing;
        }

        public async Task<PagedList<Listing>> GetListings(PaginationParams paginationParams, FilterParams filterParams, int userId = -1)
        {
            var listings = _context.Listings.AsQueryable();

            if (userId != -1)
            {
                if (filterParams.MyListingsOnly)
                {
                    listings = listings.Where(l => l.UserId == userId);
                }
                else
                {
                    listings = listings.Where(l => l.UserId != userId);
                    if (filterParams.WatchlistOnly)
                    {
                        var watchlistIds = await GetWatchingIds(userId);
                        listings = listings.Where(l => watchlistIds.Contains(l.id));
                    }
                }
            }

            listings = listings.Where(l => l.Category.ToLower().Contains(filterParams.Category.ToLower()));
            listings = listings.Where(l => l.Title.ToLower().Contains(filterParams.Title.ToLower()));

            if (filterParams.DateAdded != null)
            {
                listings = listings.Where(l => l.DateAdded > filterParams.DateAdded);
            }
            switch (paginationParams.SortBy)
            {
                case "dateAdded":
                    listings = listings.OrderBy(l => l.DateAdded);
                    break;
                case "dateAdded_desc":
                    listings = listings.OrderByDescending(l => l.DateAdded);
                    break;
                case "price":
                    listings = listings.OrderBy(l => l.Price);
                    break;
                case "price_desc":
                    listings = listings.OrderByDescending(l => l.Price);
                    break;
            }

            return await PagedList<Listing>.CreateAsync(listings, paginationParams.PageNumber, paginationParams.PageSize);
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

        public async Task<ListingWatch> GetListingWatch(int userId, int listingId)
        {
            return await _context.ListingWatchs.FirstOrDefaultAsync(u => u.WatcherId == userId && u.WatchingId == listingId);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "inbox":
                    messages = messages.Where(m => messageParams.UserId == m.RecipientId && !m.RecipientDeleted);
                    break;
                case "outbox":
                    messages = messages.Where(m => messageParams.UserId == m.SenderId && !m.SenderDeleted);
                    break;
                default:
                    messages = messages.Where(m => messageParams.UserId == m.RecipientId && !m.IsRead && !m.RecipientDeleted);
                    break;
            }

            messages = messages.OrderByDescending(m => m.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages.Where(m => (m.SenderId == userId && m.RecipientId == recipientId && !m.SenderDeleted) ||
                 (m.RecipientId == userId && m.SenderId == recipientId && !m.RecipientDeleted))
                 .OrderByDescending(m => m.MessageSent)
                 .ToListAsync();

            return messages;
        }
    }
}