namespace SYMB2C.Feature.Login
{
    public class Global : Sitecore.Web.Application
    {
        // Patch issue with Azure AD Integration. Must have a session variable during callback to catch AD token and authentication.

        // Error on CD due to redis session integration... this was getting called before redis was ready.
        // Added ProdCD symbol to project properties for ProdCD build configuration profile.
        // #if (!ProdCD)
        protected void Session_Start(object sender, System.EventArgs e)
        {
            Session["Workaround"] = 0;
        }
        // #endif
    }
}