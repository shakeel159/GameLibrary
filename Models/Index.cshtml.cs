using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Data;

namespace GameLibrary.Models
{
    public class IndexModel : PageModel
    {
        private readonly GameLibrary.Data.GameLibraryContext _context;

        public IndexModel(GameLibrary.Data.GameLibraryContext context)
        {
            _context = context;
        }

        public IList<YourList> YourList { get;set; } = default!;

        public async Task OnGetAsync()
        {
            YourList = await _context.YourList.ToListAsync();
        }
    }
}
