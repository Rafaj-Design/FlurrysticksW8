﻿

#pragma checksum "C:\Users\fuerte\Dropbox\projects\Windows8_Projects\FlurrysticksW8\Flurrysticks\AppMetrics.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "65BAC6E7C85B7C8F5240B976F83A8C85"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Flurrystics
{
    partial class AppMetrics : global::Flurrystics.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 112 "..\..\AppMetrics.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.flipView1_SelectionChanged_1;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 347 "..\..\AppMetrics.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.setClick_Click_1;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 348 "..\..\AppMetrics.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.cancelClick_Click_1;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 170 "..\..\AppMetrics.xaml"
                ((global::Telerik.UI.Xaml.Controls.Chart.ChartTrackBallBehavior)(target)).TrackInfoUpdated += this.ChartTrackBallBehavior_TrackInfoUpdated_1;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 81 "..\..\AppMetrics.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 90 "..\..\AppMetrics.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.StackPanel_Tapped_1;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 363 "..\..\AppMetrics.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.TimeRangeButton_Click_1;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


