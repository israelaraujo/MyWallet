using Newtonsoft.Json;
using System.Web.Security;

namespace MyWallet.Web.Util
{
    public class CookieUtil
    {
        private static string BuildUserToken(string userId, string userName, string mainContextId)
        {
            var userToken = new UserToken()
            {
                UserId = userId,
                Name = userName,
                MainContextId = mainContextId
            };

            var jsonToken = JsonConvert.SerializeObject(userToken);
            return jsonToken;
        }

        public static void SetAuthCookie(string userId, string userName, string mainContextId, bool? rememberMe = null)
        {
            string jsonToken = BuildUserToken(userId, userName, mainContextId);
            FormsAuthentication.SetAuthCookie(jsonToken, rememberMe.GetValueOrDefault());
        }
    }
}