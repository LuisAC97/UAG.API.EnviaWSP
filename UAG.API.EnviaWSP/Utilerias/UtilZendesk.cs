using System.Text;

namespace UAG.API.EnviaWSP.Utilerias
{
    public class UtilZendesk
    {
        public static string GetEncoded(IConfiguration config)
        {
            string sEncoded;
            try
            {
                string username = config.GetValue<string>("ApiZendeskSettings:usernameZendesk");
                string password = config.GetValue<string>("ApiZendeskSettings:passwordZendesk");
                sEncoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            }
            catch (Exception ex)
            {

                throw;
            }
            return sEncoded;
        }

        public static string GetSuncoEncoded(IConfiguration config)
        {
            string sEncoded;
            try
            {
                string username = config.GetValue<string>("SuncoAPI:userKey");
                string password = config.GetValue<string>("SuncoAPI:userPass");
                sEncoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            }
            catch (Exception ex)
            {

                throw;
            }
            return sEncoded;
        }

        public static string GetURL(IConfiguration config)
        {
            string sURL;
            try
            {
                sURL = String.Format(config.GetValue<string>("ApiZendeskSettings:apiZendesk"));
            }
            catch (Exception ex)
            {

                throw;
            }
            return sURL;
        }

        public static string GetCredZendesk(IConfiguration config)
        {
            string sCred;
            try
            {
                string email = config.GetValue<string>("ApiToken:email");
                string password = config.GetValue<string>("ApiToken:password");
                sCred = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + password));
            }
            catch (Exception e)
            {
                throw;
            }
            return sCred;
        }
    }
}
