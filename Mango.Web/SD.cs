namespace Mango.Web
{
    public class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static string ProductAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string CouponAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
