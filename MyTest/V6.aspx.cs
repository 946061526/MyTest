using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyTest
{
    public partial class V6 : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {

        }
    }



    public class Points
    {
        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }

        public double distinct
        {
            get
            {
                return Math.Sqrt( X * X ) + Y * Y;
            }
        }

//        public double distinct=>Math


        public static async void RunAsync( Action function, Action callback )
        {
            Func<System.Threading.Tasks.Task> taskFunc = () =>
            {
                return System.Threading.Tasks.Task.Run( () =>
                {
                    function();
                } );
            };
            await taskFunc();
            if ( callback != null )
                callback();
        }  

    }
}