using System;
using Appcharge.PaymentLinks.Models;
using UnityEngine;

namespace Appcharge.PaymentLinks.Platforms.Android
{
	public static class OrderResponseModelConverter
	{
		public static OrderResponseModel ToOrderResponseModel(AndroidJavaObject javaOrderResponse)
		{
			if (javaOrderResponse == null) return null;

			long date = GetSafeLong(javaOrderResponse, "date")
						?? ParseLong(GetSafeString(javaOrderResponse, "date"))
						?? 0L;

			int price = GetSafeInt(javaOrderResponse, "price") ?? 0;

			return new OrderResponseModel
			{
				date = date,
				sessionToken = GetSafeString(javaOrderResponse, "sessionToken") ?? string.Empty,
				offerName = GetSafeString(javaOrderResponse, "offerName") ?? string.Empty,
				offerSku = GetSafeString(javaOrderResponse, "offerSku") ?? string.Empty,
				items = GetItems(javaOrderResponse.Get<AndroidJavaObject>("items")),
				price = price,
				currency = GetSafeString(javaOrderResponse, "currency") ?? string.Empty,
				customerId = GetSafeString(javaOrderResponse, "customerId") ?? string.Empty,
				customerCountry = GetSafeString(javaOrderResponse, "customerCountry") ?? string.Empty,
				paymentMethodName = GetSafeString(javaOrderResponse, "paymentMethodName") ?? string.Empty,
				orderId = GetSafeString(javaOrderResponse, "orderId") ?? string.Empty,
				purchaseId = GetSafeString(javaOrderResponse, "purchaseId") ?? string.Empty
			};
		}

		private static string GetSafeString(AndroidJavaObject obj, string fieldName)
		{
			if (obj == null) return null;
			try { return obj.Get<string>(fieldName); }
			catch { return null; }
		}

		private static string GetQuantityString(AndroidJavaObject javaProduct)
		{
			if (javaProduct == null)
				return string.Empty;

			var s = GetSafeString(javaProduct, "quantity");
			if (!string.IsNullOrEmpty(s))
				return s;

			return string.Empty;
		}

		private static int? GetSafeInt(AndroidJavaObject obj, string fieldName)
		{
			if (obj == null) return null;
			try { return obj.Get<int>(fieldName); }
			catch
			{
				try { return (int)obj.Get<long>(fieldName); } catch { }
				try { return (int)obj.Get<double>(fieldName); } catch { }
				var s = GetSafeString(obj, fieldName);
				if (int.TryParse(s, out var i)) return i;
				if (double.TryParse(s, System.Globalization.NumberStyles.Any,
									System.Globalization.CultureInfo.InvariantCulture, out var d))
					return (int)d;
				return null;
			}
		}

		private static long? GetSafeLong(AndroidJavaObject obj, string fieldName)
		{
			if (obj == null) return null;
			try { return obj.Get<long>(fieldName); }
			catch
			{
				try { return (long)obj.Get<int>(fieldName); } catch { }
				try { return (long)obj.Get<double>(fieldName); } catch { }
				var s = GetSafeString(obj, fieldName);
				return ParseLong(s);
			}
		}

		private static long? ParseLong(string s)
		{
			if (string.IsNullOrEmpty(s)) return null;
			if (long.TryParse(s, out var l)) return l;
			if (double.TryParse(s, System.Globalization.NumberStyles.Any,
								System.Globalization.CultureInfo.InvariantCulture, out var d))
				return (long)d;
			return null;
		}

		private static ProductModel[] GetItems(AndroidJavaObject javaItemsList)
		{
			if (javaItemsList == null) return Array.Empty<ProductModel>();

			int size = 0;
			try { size = javaItemsList.Call<int>("size"); }
			catch { return Array.Empty<ProductModel>(); }

			var items = new ProductModel[size];
			for (int i = 0; i < size; i++)
			{
				AndroidJavaObject javaProduct = null;
				try { javaProduct = javaItemsList.Call<AndroidJavaObject>("get", i); }
				catch { }

				if (javaProduct == null)
				{
					items[i] = new ProductModel { name = string.Empty, sku = string.Empty, amount = 0.ToString() };
					continue;
				}

				var name = GetSafeString(javaProduct, "name") ?? string.Empty;
				var sku = GetSafeString(javaProduct, "sku") ?? string.Empty;
				var quantityStr = GetQuantityString(javaProduct);

				items[i] = new ProductModel
				{
					name = name,
					sku = sku,
					amount = quantityStr
				};
			}

			return items;
		}
	}
}