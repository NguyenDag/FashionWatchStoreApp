﻿#pragma checksum "..\..\..\ProductDetail.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "674957A9515CC395CA19D57EE39FA277799C14E1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ProjectPRN;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ProjectPRN {
    
    
    /// <summary>
    /// ProductDetail
    /// </summary>
    public partial class ProductDetail : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 42 "..\..\..\ProductDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer MainScrollViewer;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\ProductDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Detail;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\ProductDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ImageList;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\ProductDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddToCart;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\ProductDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SortReview;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\ProductDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ReviewList;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\..\ProductDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox RelatedProductList;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProjectPRN;component/productdetail.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ProductDetail.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainScrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 2:
            this.Detail = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.ImageList = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            this.btnAddToCart = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\..\ProductDetail.xaml"
            this.btnAddToCart.Click += new System.Windows.RoutedEventHandler(this.btnAddToCart_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SortReview = ((System.Windows.Controls.ComboBox)(target));
            
            #line 101 "..\..\..\ProductDetail.xaml"
            this.SortReview.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SortReview_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ReviewList = ((System.Windows.Controls.ListBox)(target));
            return;
            case 7:
            this.RelatedProductList = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

