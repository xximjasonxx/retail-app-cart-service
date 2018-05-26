
using System;

namespace CartApi.Common
{
    public class CartNotFoundException : Exception
    {
        public CartNotFoundException(string cartKey) : base($"Cart Not Found: {cartKey}")
        {
            
        }
    }
}