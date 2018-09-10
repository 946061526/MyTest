using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceStack.Redis;

namespace MyTest.nosql
{
    public partial class testNosql : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            redis();
        }
        private void redis()
        {
            RedisClient redisClient = new RedisClient( "127.0.0.1", 6379 );
            redisClient.Set<string>( "key001", "hello C#" );

            Console.Write( redisClient.Get<string>( "key001" ) );
        }
    }
}