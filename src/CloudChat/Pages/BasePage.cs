using Dna;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CloudChat
{
    public class BasePage : UserControl
    {
        #region Private Memebers

        private object mViewModel;
        #endregion

        #region Public Properties

        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;
        public PageAnimation PageUnLoadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;
        public float SlideSeconds { get; set; } = 0.3f;
        public bool ShouldAnimateOut { get; set; }

        public object ViewModelObject
        {
            get => mViewModel;
            set
            {
                if (mViewModel == value)
                    return;

                mViewModel = value;
                OnViewModelChanged();
                DataContext = mViewModel;
            }
        }
        #endregion

        #region Constructor

        public BasePage()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            if (PageLoadAnimation != PageAnimation.None)
                Visibility = Visibility.Collapsed;
            Loaded += BasePage_LoadedAsync;
        }

        #endregion

        public async void BasePage_LoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ShouldAnimateOut)
                await AnimateOutAsync();
            else
                await AnimateInAsync();
        }

        public async Task AnimateInAsync()
        {
            if (PageLoadAnimation == PageAnimation.None)
                return;

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:
                    await this.SlideAndFadeInAsync(AnimationSlideInDirection.Right, false, SlideSeconds, size: (int)Application.Current.MainWindow.Width / 10);
                    break;

                default:
                    break;
            }
        }

        public async Task AnimateOutAsync()
        {
            if (PageUnLoadAnimation == PageAnimation.None)
                return;

            switch (PageUnLoadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToLeft:
                    await this.SlideAndFadeOutAsync(AnimationSlideInDirection.Left, SlideSeconds, size: (int)Application.Current.MainWindow.Width / 10);
                    break;

                default:
                    break;
            }
        }

        protected virtual void OnViewModelChanged()
        {
        }
    }

    public class BasePage<VM> : BasePage where VM : BaseViewModel, new()
    {
        public VM ViewModel
        {
            get => (VM)ViewModelObject;
            set => ViewModelObject = value;
        }

        #region Constructor

        public BasePage() : base()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                ViewModel = new VM();
            else
                ViewModel = Framework.Service<VM>() ?? new VM();
        }

        public BasePage(VM specificViewModel = null) : base()
        {
            if (specificViewModel != null)
                ViewModel = specificViewModel;
            else
            {
                if (DesignerProperties.GetIsInDesignMode(this))
                    ViewModel = new VM();
                else
                    ViewModel = Framework.Service<VM>() ?? new VM();
            }
        }
        #endregion


    }
}