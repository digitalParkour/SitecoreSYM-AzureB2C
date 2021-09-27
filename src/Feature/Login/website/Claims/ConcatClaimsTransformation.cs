using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Services;

namespace SYMB2C.Feature.Login.Claims
{
    public class ConcatClaimsTransformation : DefaultTransformation
    {
        private string delimeter;

        public ConcatClaimsTransformation()
            : base()
        {
            this.delimeter = " ";
        }

        public ConcatClaimsTransformation(string delimeter)
            : base()
        {
            this.delimeter = string.IsNullOrWhiteSpace(delimeter) ? " " : delimeter;
        }

        public override void Transform(ClaimsIdentity identity, TransformationContext context)
        {
            Assert.ArgumentNotNull(identity, "identity");
            Assert.ArgumentNotNull(context, "context");
            List<Claim> claims = new List<Claim>();
            foreach (ClaimInfo source in this.Source)
            {
                List<Claim> list = identity.Claims.Where(c =>
                {
                    if (!string.Equals(c.Type, source.Name, StringComparison.Ordinal))
                    {
                        return false;
                    }

                    if (!source.HasValue)
                    {
                        return true;
                    }

                    return string.Equals( c.Value,source.Value, StringComparison.Ordinal);
                }).ToList();

                if (list.Count != 0)
                {
                    foreach (var claim in list)
                    {
                        claims.Add(claim);
                    }
                }
                else
                {
                    return;
                }
            }

            if (!this.KeepSource)
            {
                ClaimsIdentity claimsIdentity = identity;
                claims.ForEach(claimsIdentity.RemoveClaim);
            }

            string claimvalue = string.Empty;
            if (claims != null && claims.Count > 0)
            {
                claimvalue = string.Join(this.delimeter, claims.Select(claim => claim.Value));

                foreach (ClaimInfo target in this.Target)
                {
                    // Remove any target claims
                    var claimsToRemove = identity.FindAll(target.Name);
                    if (claimsToRemove != null && claims.Any())
                    {
                        foreach (var removeMe in claimsToRemove)
                        {
                            identity.RemoveClaim(removeMe);
                        }
                    }

                    // Set target claims
                    Claim claim2 = new Claim(target.Name, (target.HasValue ? target.Value : claimvalue));
                    if (identity.Claims.Any<Claim>((Claim c) =>
                    {
                        if (!string.Equals(c.Type, claim2.Type, StringComparison.Ordinal))
                        {
                            return false;
                        }

                        return string.Equals(c.Value, claim2.Value, StringComparison.Ordinal);
                    }))
                    {
                        continue;
                    }

                    identity.AddClaim(claim2);
                }
            }
        }
    }
}