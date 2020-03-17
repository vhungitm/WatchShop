using System.Web;
using System.Web.Optimization;

namespace WatchShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/client/css").Include(
                    "~/assets/admin/css/sb-admin-2.css",
                    "~/assets/client/css/animate.css",
                    "~/assets/client/css/owl.carousel.min.css",
                    "~/assets/admin/vendor/fontawesome-free/css/all.min.css",
                    "~/assets/client/css/nice-select.css",
                    "~/assets/client/css/flaticon.css",
                    "~/assets/client/css/themify-icons.css",
                    "~/assets/client/css/magnific-popup.css",
                    "~/assets/client/css/slick.css",
                    "~/assets/client/css/price_rangs.css",
                    "~/assets/client/css/style.css",
                    "~/assets/client/css/jquery-ui.css"
                 )
            );

            bundles.Add(new ScriptBundle("~/bundles/client/script").Include(
                    "~/Assets/Client/js/jquery-ui.js",
                    "~/Assets/Client/js/controller/baseController.js",
                    "~/Assets/Admin/js/sb-admin-2.js",
                    "~/Assets/Client/js/popper.min.js",
                    "~/Assets/Client/js/bootstrap.min.js",
                    "~/Assets/Client/js/jquery.magnific-popup.js",
                    "~/Assets/Client/js/swiper.min.js",
                    "~/Assets/Client/js/lightslider.min.js",
                    "~/Assets/Client/js/mixitup.min.js",
                    "~/Assets/Client/js/owl.carousel.min.js",
                    "~/Assets/Client/js/jquery.nice-select.min.js",
                    "~/Assets/Client/js/slick.min.js",
                    "~/Assets/Client/js/jquery.counterup.min.js",
                    "~/Assets/Client/js/waypoints.min.js",
                    "~/Assets/Client/js/contact.js",
                    "~/Assets/Client/js/jquery.ajaxchimp.min.js",
                    "~/Assets/Client/js/jquery.form.js",
                    "~/Assets/Client/js/jquery.validate.min.js",
                    "~/Assets/Client/js/mail-script.js",
                    "~/Assets/Client/js/custom.js"
                )
            );

            bundles.Add(new ScriptBundle("~/bundles/admin/script").Include(
                    "~/Assets/Admin/vendor/jquery/jquery.min.js",
                    "~/Assets/Admin/vendor/bootstrap/js/bootstrap.bundle.min.js",
                    "~/Assets/Admin/vendor/jquery-easing/jquery.easing.min.js",
                    "~/Assets/Admin/js/plugins/ckfinder/ckfinder.js",
                    "~/Assets/Admin/js/plugins/ckeditor/ckeditor.js",
                    "~/Assets/Admin/js/sb-admin-2.js"
                )
            );
            
        }
    }
}