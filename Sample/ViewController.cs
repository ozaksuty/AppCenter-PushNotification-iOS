using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using System;
using UIKit;
using WebKit;

namespace Sample
{
    public partial class ViewController : UIViewController
    {
		public ViewController (IntPtr handle) : base (handle)
		{
            Push.PushNotificationReceived += Push_PushNotificationReceived;
		}

        private void Push_PushNotificationReceived(object sender, PushNotificationReceivedEventArgs e)
        {
            UIAlertController actionSheetAlert = UIAlertController.
                Create(e.Title, e.Message, UIAlertControllerStyle.ActionSheet);

            //Add Custom Actions
            var extData = e.CustomData;
            if (extData.Count > 0)
                foreach (var item in extData)
                    actionSheetAlert.AddAction(UIAlertAction.
                        Create(item.Key, UIAlertActionStyle.Default, 
                        (action) =>
                        {
                            WKWebView webView = new WKWebView(View.Frame, new WKWebViewConfiguration());
                            View.AddSubview(webView);

                            var url = new NSUrl(item.Value);
                            var request = new NSUrlRequest(url);
                            webView.LoadRequest(request);
                        }));

            UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = this.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            //Display
            this.PresentViewController(actionSheetAlert, true, null);
        }

        public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
    }
}