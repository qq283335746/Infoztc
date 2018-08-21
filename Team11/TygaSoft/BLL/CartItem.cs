using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    [Serializable]
    public partial class CartItem
    {
        private Dictionary<Guid, CartItemInfo> cartItems = new Dictionary<Guid, CartItemInfo>();

        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (CartItemInfo item in cartItems.Values)
                    total += item.Price * item.Quantity;
                return total;
            }
        }

        public void SetQuantity(Guid itemId, int qty)
        {
            cartItems[itemId].Quantity = qty;
        }

        public int Count
        {
            get { return cartItems.Count; }
        }

        public void Add(Guid itemId)
        {
            CartItemInfo cartItem;
            if (!cartItems.TryGetValue(itemId, out cartItem))
            {
                Product bll = new Product();
                ProductInfo data = bll.GetModel(itemId);
                if (data != null)
                {
                    CartItemInfo newItem = new CartItemInfo { ProductId = itemId, Named = data.Named, CategoryId = data.CategoryId, Price = data.Price, Quantity = 1, LastUpdatedDate = DateTime.Now };
                    cartItems.Add(itemId, newItem);
                }
            }
            else
                cartItem.Quantity++;
        }

        public void Add(CartItemInfo item)
        {
            CartItemInfo cartItem;
            if (!cartItems.TryGetValue(Guid.Parse(item.ProductId.ToString()), out cartItem))
                cartItems.Add(Guid.Parse(item.ProductId.ToString()), item);
            else
                cartItem.Quantity += item.Quantity;
        }

        public void Remove(Guid itemId)
        {
            cartItems.Remove(itemId);
        }

        public ICollection<CartItemInfo> CartItems
        {
            get { return cartItems.Values; }
        }

        public LineItemInfo[] GetOrderLineItems()
        {
            LineItemInfo[] orderLineItems = new LineItemInfo[cartItems.Count];
            int lineNum = 0;

            foreach (CartItemInfo item in cartItems.Values)
                orderLineItems[lineNum] = new LineItemInfo { ProductId = item.ProductId, ProductName = item.Named, Line = ++lineNum, Quantity = item.Quantity, Price = item.Price };

            return orderLineItems;
        }

        public void Clear()
        {
            cartItems.Clear();
        }
    }
}
