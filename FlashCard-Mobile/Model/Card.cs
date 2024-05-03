using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCard_Mobile.Model
{
    public class Card
    {
        public int Id { get; set; }
        public string? Recto { get; set; }
        public string? Verso { get; set; }
    }
}
