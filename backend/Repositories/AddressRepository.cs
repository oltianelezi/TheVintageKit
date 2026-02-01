using backend.Data;
using backend.Models;

namespace backend.Repositories
{
    public class AddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateNewAddress(Address NewAddress)
        {
            await _context.Addresses.AddAsync(NewAddress);
            await _context.SaveChangesAsync();

            return (int)NewAddress.AddressId;
        }
    }
}
