using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using positus_api_csharp_client;
using positus_api_csharp_client.Models.Requests;
using positus_api_csharp_client.Models.Responses;

namespace SOSTransito.Repositories
{
    public class whatsapp
    {
        public static bool sendWpp(string clientTel, string texto)
        {
            try
            {
                Client client = new Client(@"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiYzJmMTg5NGNmNTU5MzUwYzlhOTAxYWNhOGEzNzA1MjhhNmVlOTNiMmNmNDY4YTcxOTE5M2Q3MTFiYjgzYWNmOGZiMzA2N2M4YzllYWM0NWIiLCJpYXQiOjE2MzE1NjA0MTUuMTAyNTk3LCJuYmYiOjE2MzE1NjA0MTUuMTAyNjAyLCJleHAiOjE2NjMwOTY0MTUuMDk4NTg5LCJzdWIiOiI0MTE3Iiwic2NvcGVzIjpbXX0.lWYD84pIoxLQJqFDvNWtc9OS0tUU5DHAqyPBilsIwYtFQfZy9m5Xo6tUMCELRoGQ0uW6WPO8XXnAIKTkiEXqsjb8oW4jnkN9i1dZHoMkOSnSGjRqYZN42aot72r6jwtz5fBMIa71k7h-z9lWU2QpfbIsJj8j9weL6bloRhAL3AX8B6Lfx6TknjSRWYo31NrEL0lVNjV5HHRTc5U3Scpemp97CRRTzNatIMuRPimaGKH0aw1O4ymVnlP3vywqK1I1A9bfsmyQjGpRruuydNbPv-eaLQt8q8tNkqhKavmMwCPoX-WpPgpcA4DH4vGjM2_QEpYjAfzVBQmtPOFwUrbG_X3YtxC45RZ9Hb1ZbQOkPIDrlZ25_xht7N32ub1_B0w0X-zyv10xX-NrJrcIly_jMxug2BVqvfrcq_T0Cpl1FW449qOuRdD2PnAUoagDWaeJnxSIXC_gchRnke8u6x3JAEMx2ndGZX1Le2-2czuy-4sB2ba2bgBJobVfJhlJSJqEkKbD5ZOr3gPjPfljuwVRvPuwqUcgxTqRGc4NpTV1tzPAM4A2rvsjZroTvv5p2SwWChVZuJU0Y5nW5C6x8l5d7fzB8CRaU3SXmDHcygIqeAjWQZ-NxaSbDrd97axvrw_Aug216yDwtPMqmkNdhejb5KVx5NaABtw5uJ-qWZ1nKuQ", true);

                client.numberId = "6334ea09-d3fe-4689-8acb-684eb0d0ec78";

                @Message message = client.sendMessage(new Text()
                {
                    text = new TextDetail() { body = texto },
                    to = clientTel,
                    type = "text"
                });
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
