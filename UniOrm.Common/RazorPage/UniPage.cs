using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace UniOrm 
{
    public class UniPage:  PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // if (!ModelState.IsValid)
            //{           
            //  return Page();
            //}
            return Page();
            //_dbContext.Categories.Add(Category);    
            //      await _dbContext.SaveChangesAsync();   
            //      return RedirectToPage("./Index");
        }
    }
}
