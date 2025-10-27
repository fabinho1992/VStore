using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewManager.Domain.ModelsAutentication
{
    public class ResponseLogin
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public DateTime Expired { get; set; }
    }
}
