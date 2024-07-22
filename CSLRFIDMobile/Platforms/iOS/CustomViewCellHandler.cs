using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using CSLRFIDMobile.Helper;

namespace CSLRFIDMobile.Platforms.iOS
{
    public class CustomViewCellHandler : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = ((CustomViewCell)item).SelectedBackgroundColor.ToPlatform()
            };

            return cell;
        }
    }
}
