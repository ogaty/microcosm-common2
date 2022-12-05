using microcosm.config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class RingCanvasViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RingCanvasViewModel(ConfigData config)
        {
            outerWidth = config.zodiacOuterWidth;
            outerHeight = config.zodiacOuterWidth;
            innerWidth = outerWidth - config.zodiacWidth;
            innerHeight = outerHeight - config.zodiacWidth;
            innerLeft = config.zodiacWidth / 2;
            innerTop = config.zodiacWidth / 2;
            centerWidth = config.zodiacCenter;
            centerHeight = config.zodiacCenter;
            centerLeft = config.zodiacOuterWidth / 2 - config.zodiacCenter / 2;
            centerTop = config.zodiacOuterWidth / 2 - config.zodiacCenter / 2;
        }


        // 獣帯外側
        #region
        private double _outerWidth;
        public double outerWidth
        {
            get
            {
                return _outerWidth;
            }
            set
            {
                _outerWidth = value;
                OnPropertyChanged("outerWidth");
            }
        }
        private double _outerHeight;
        public double outerHeight
        {
            get
            {
                return _outerHeight;
            }
            set
            {
                _outerHeight = value;
                OnPropertyChanged("outerHeight");
            }
        }
        #endregion

        // 獣帯内側
        #region
        private double _innerWidth;
        public double innerWidth
        {
            get
            {
                return _innerWidth;
            }
            set
            {
                _innerWidth = value;
                OnPropertyChanged("innerWidth");
            }
        }
        private double _innerHeight;
        public double innerHeight
        {
            get
            {
                return _innerHeight;
            }
            set
            {
                _innerHeight = value;
                OnPropertyChanged("innerHeight");
            }
        }
        private double _innerLeft;
        public double innerLeft
        {
            get
            {
                return _innerLeft;
            }
            set
            {
                _innerLeft = value;
                OnPropertyChanged("innerLeft");
            }
        }
        private double _innerTop;
        public double innerTop
        {
            get
            {
                return _innerTop;
            }
            set
            {
                _innerTop = value;
                OnPropertyChanged("innerTop");
            }
        }
        #endregion

        // 中心
        #region
        private double _centerWidth;
        public double centerWidth
        {
            get
            {
                return _centerWidth;
            }
            set
            {
                _centerWidth = value;
                OnPropertyChanged("centerWidth");
            }
        }

        private double _centerHeight;
        public double centerHeight
        {
            get
            {
                return _centerHeight;
            }
            set
            {
                _centerHeight = value;
                OnPropertyChanged("centerHeight");
            }
        }
        private double _centerLeft;
        public double centerLeft
        {
            get
            {
                return _centerLeft;
            }
            set
            {
                _centerLeft = value;
                OnPropertyChanged("centerLeft");
            }
        }
        private double _centerTop;
        public double centerTop
        {
            get
            {
                return _centerTop;
            }
            set
            {
                _centerTop = value;
                OnPropertyChanged("centerTop");
            }
        }
        #endregion

        // カスプ1
        #region
        private double _cusp1x1;
        public double cusp1x1
        {
            get
            {
                return _cusp1x1;
            }
            set
            {
                _cusp1x1 = value;
                OnPropertyChanged("cusp1x1");
            }
        }

        private double _cusp1y1;
        public double cusp1y1
        {
            get
            {
                return _cusp1y1;
            }
            set
            {
                _cusp1y1 = value;
                OnPropertyChanged("cusp1y1");
            }
        }

        private double _cusp1x2;
        public double cusp1x2
        {
            get
            {
                return _cusp1x2;
            }
            set
            {
                _cusp1x2 = value;
                OnPropertyChanged("cusp1x2");
            }
        }

        private double _cusp1y2;
        public double cusp1y2
        {
            get
            {
                return _cusp1y2;
            }
            set
            {
                _cusp1y2 = value;
                OnPropertyChanged("cusp1y2");
            }
        }

        #endregion

        // カスプ2
        #region
        private double _cusp2x1;
        public double cusp2x1
        {
            get
            {
                return _cusp2x1;
            }
            set
            {
                _cusp2x1 = value;
                OnPropertyChanged("cusp2x1");
            }
        }

        private double _cusp2y1;
        public double cusp2y1
        {
            get
            {
                return _cusp2y1;
            }
            set
            {
                _cusp2y1 = value;
                OnPropertyChanged("cusp2y1");
            }
        }

        private double _cusp2x2;
        public double cusp2x2
        {
            get
            {
                return _cusp2x2;
            }
            set
            {
                _cusp2x2 = value;
                OnPropertyChanged("cusp2x2");
            }
        }

        private double _cusp2y2;
        public double cusp2y2
        {
            get
            {
                return _cusp2y2;
            }
            set
            {
                _cusp2y2 = value;
                OnPropertyChanged("cusp2y2");
            }
        }

        #endregion

        // カスプ3
        #region
        private double _cusp3x1;
        public double cusp3x1
        {
            get
            {
                return _cusp3x1;
            }
            set
            {
                _cusp3x1 = value;
                OnPropertyChanged("cusp3x1");
            }
        }

        private double _cusp3y1;
        public double cusp3y1
        {
            get
            {
                return _cusp3y1;
            }
            set
            {
                _cusp3y1 = value;
                OnPropertyChanged("cusp3y1");
            }
        }

        private double _cusp3x2;
        public double cusp3x2
        {
            get
            {
                return _cusp3x2;
            }
            set
            {
                _cusp3x2 = value;
                OnPropertyChanged("cusp3x2");
            }
        }

        private double _cusp3y2;
        public double cusp3y2
        {
            get
            {
                return _cusp3y2;
            }
            set
            {
                _cusp3y2 = value;
                OnPropertyChanged("cusp3y2");
            }
        }

        #endregion

        // カスプ4
        #region
        private double _cusp4x1;
        public double cusp4x1
        {
            get
            {
                return _cusp4x1;
            }
            set
            {
                _cusp4x1 = value;
                OnPropertyChanged("cusp4x1");
            }
        }

        private double _cusp4y1;
        public double cusp4y1
        {
            get
            {
                return _cusp4y1;
            }
            set
            {
                _cusp4y1 = value;
                OnPropertyChanged("cusp4y1");
            }
        }

        private double _cusp4x2;
        public double cusp4x2
        {
            get
            {
                return _cusp4x2;
            }
            set
            {
                _cusp4x2 = value;
                OnPropertyChanged("cusp4x2");
            }
        }

        private double _cusp4y2;
        public double cusp4y2
        {
            get
            {
                return _cusp4y2;
            }
            set
            {
                _cusp4y2 = value;
                OnPropertyChanged("cusp4y2");
            }
        }

        #endregion

        // カスプ5
        #region
        private double _cusp5x1;
        public double cusp5x1
        {
            get
            {
                return _cusp5x1;
            }
            set
            {
                _cusp5x1 = value;
                OnPropertyChanged("cusp5x1");
            }
        }

        private double _cusp5y1;
        public double cusp5y1
        {
            get
            {
                return _cusp5y1;
            }
            set
            {
                _cusp5y1 = value;
                OnPropertyChanged("cusp5y1");
            }
        }

        private double _cusp5x2;
        public double cusp5x2
        {
            get
            {
                return _cusp5x2;
            }
            set
            {
                _cusp5x2 = value;
                OnPropertyChanged("cusp5x2");
            }
        }

        private double _cusp5y2;
        public double cusp5y2
        {
            get
            {
                return _cusp5y2;
            }
            set
            {
                _cusp5y2 = value;
                OnPropertyChanged("cusp5y2");
            }
        }

        #endregion

        // カスプ6
        #region
        private double _cusp6x1;
        public double cusp6x1
        {
            get
            {
                return _cusp6x1;
            }
            set
            {
                _cusp6x1 = value;
                OnPropertyChanged("cusp6x1");
            }
        }

        private double _cusp6y1;
        public double cusp6y1
        {
            get
            {
                return _cusp6y1;
            }
            set
            {
                _cusp6y1 = value;
                OnPropertyChanged("cusp6y1");
            }
        }

        private double _cusp6x2;
        public double cusp6x2
        {
            get
            {
                return _cusp6x2;
            }
            set
            {
                _cusp6x2 = value;
                OnPropertyChanged("cusp6x2");
            }
        }

        private double _cusp6y2;
        public double cusp6y2
        {
            get
            {
                return _cusp6y2;
            }
            set
            {
                _cusp6y2 = value;
                OnPropertyChanged("cusp6y2");
            }
        }

        #endregion

        // カスプ7
        #region
        private double _cusp7x1;
        public double cusp7x1
        {
            get
            {
                return _cusp7x1;
            }
            set
            {
                _cusp7x1 = value;
                OnPropertyChanged("cusp7x1");
            }
        }

        private double _cusp7y1;
        public double cusp7y1
        {
            get
            {
                return _cusp7y1;
            }
            set
            {
                _cusp7y1 = value;
                OnPropertyChanged("cusp7y1");
            }
        }

        private double _cusp7x2;
        public double cusp7x2
        {
            get
            {
                return _cusp7x2;
            }
            set
            {
                _cusp7x2 = value;
                OnPropertyChanged("cusp7x2");
            }
        }

        private double _cusp7y2;
        public double cusp7y2
        {
            get
            {
                return _cusp7y2;
            }
            set
            {
                _cusp7y2 = value;
                OnPropertyChanged("cusp7y2");
            }
        }

        #endregion

        // カスプ8
        #region
        private double _cusp8x1;
        public double cusp8x1
        {
            get
            {
                return _cusp8x1;
            }
            set
            {
                _cusp8x1 = value;
                OnPropertyChanged("cusp8x1");
            }
        }

        private double _cusp8y1;
        public double cusp8y1
        {
            get
            {
                return _cusp8y1;
            }
            set
            {
                _cusp8y1 = value;
                OnPropertyChanged("cusp8y1");
            }
        }

        private double _cusp8x2;
        public double cusp8x2
        {
            get
            {
                return _cusp8x2;
            }
            set
            {
                _cusp8x2 = value;
                OnPropertyChanged("cusp8x2");
            }
        }

        private double _cusp8y2;
        public double cusp8y2
        {
            get
            {
                return _cusp8y2;
            }
            set
            {
                _cusp8y2 = value;
                OnPropertyChanged("cusp8y2");
            }
        }

        #endregion

        // カスプ9
        #region
        private double _cusp9x1;
        public double cusp9x1
        {
            get
            {
                return _cusp9x1;
            }
            set
            {
                _cusp9x1 = value;
                OnPropertyChanged("cusp9x1");
            }
        }

        private double _cusp9y1;
        public double cusp9y1
        {
            get
            {
                return _cusp9y1;
            }
            set
            {
                _cusp9y1 = value;
                OnPropertyChanged("cusp9y1");
            }
        }

        private double _cusp9x2;
        public double cusp9x2
        {
            get
            {
                return _cusp9x2;
            }
            set
            {
                _cusp9x2 = value;
                OnPropertyChanged("cusp9x2");
            }
        }

        private double _cusp9y2;
        public double cusp9y2
        {
            get
            {
                return _cusp9y2;
            }
            set
            {
                _cusp9y2 = value;
                OnPropertyChanged("cusp9y2");
            }
        }

        #endregion

        // カスプ10
        #region
        private double _cusp10x1;
        public double cusp10x1
        {
            get
            {
                return _cusp10x1;
            }
            set
            {
                _cusp10x1 = value;
                OnPropertyChanged("cusp10x1");
            }
        }

        private double _cusp10y1;
        public double cusp10y1
        {
            get
            {
                return _cusp10y1;
            }
            set
            {
                _cusp10y1 = value;
                OnPropertyChanged("cusp10y1");
            }
        }

        private double _cusp10x2;
        public double cusp10x2
        {
            get
            {
                return _cusp10x2;
            }
            set
            {
                _cusp10x2 = value;
                OnPropertyChanged("cusp10x2");
            }
        }

        private double _cusp10y2;
        public double cusp10y2
        {
            get
            {
                return _cusp10y2;
            }
            set
            {
                _cusp10y2 = value;
                OnPropertyChanged("cusp10y2");
            }
        }

        #endregion

        // カスプ11
        #region
        private double _cusp11x1;
        public double cusp11x1
        {
            get
            {
                return _cusp11x1;
            }
            set
            {
                _cusp11x1 = value;
                OnPropertyChanged("cusp11x1");
            }
        }

        private double _cusp11y1;
        public double cusp11y1
        {
            get
            {
                return _cusp11y1;
            }
            set
            {
                _cusp11y1 = value;
                OnPropertyChanged("cusp11y1");
            }
        }

        private double _cusp11x2;
        public double cusp11x2
        {
            get
            {
                return _cusp11x2;
            }
            set
            {
                _cusp11x2 = value;
                OnPropertyChanged("cusp11x2");
            }
        }

        private double _cusp11y2;
        public double cusp11y2
        {
            get
            {
                return _cusp11y2;
            }
            set
            {
                _cusp11y2 = value;
                OnPropertyChanged("cusp11y2");
            }
        }

        #endregion

        // カスプ12
        #region
        private double _cusp12x1;
        public double cusp12x1
        {
            get
            {
                return _cusp12x1;
            }
            set
            {
                _cusp12x1 = value;
                OnPropertyChanged("cusp12x1");
            }
        }

        private double _cusp12y1;
        public double cusp12y1
        {
            get
            {
                return _cusp12y1;
            }
            set
            {
                _cusp12y1 = value;
                OnPropertyChanged("cusp12y1");
            }
        }

        private double _cusp12x2;
        public double cusp12x2
        {
            get
            {
                return _cusp12x2;
            }
            set
            {
                _cusp12x2 = value;
                OnPropertyChanged("cusp12x2");
            }
        }

        private double _cusp12y2;
        public double cusp12y2
        {
            get
            {
                return _cusp12y2;
            }
            set
            {
                _cusp12y2 = value;
                OnPropertyChanged("cusp12y2");
            }
        }

        #endregion


        // 牡羊座
        #region
        private double _ariesX;
        public double ariesX
        {
            get
            {
                return _ariesX;
            }
            set
            {
                _ariesX = value;
                OnPropertyChanged("ariesx");
            }
        }

        private double _ariesY;
        public double ariesY
        {
            get
            {
                return _ariesY;
            }
            set
            {
                _ariesY = value;
                OnPropertyChanged("ariesy");
            }
        }

        private string _ariesTxt;
        public string ariesTxt
        {
            get
            {
                return _ariesTxt;
            }
            set
            {
                _ariesTxt = value;
                OnPropertyChanged("ariesTxt");
            }
        }

        #endregion

        // 牡牛座
        #region
        private double _taurusX;
        public double taurusX
        {
            get
            {
                return _taurusX;
            }
            set
            {
                _taurusX = value;
                OnPropertyChanged("taurusx");
            }
        }

        private double _taurusY;
        public double taurusY
        {
            get
            {
                return _taurusY;
            }
            set
            {
                _taurusY = value;
                OnPropertyChanged("taurusy");
            }
        }
        private string _taurusTxt;
        public string taurusTxt
        {
            get
            {
                return _taurusTxt;
            }
            set
            {
                _taurusTxt = value;
                OnPropertyChanged("taurusTxt");
            }
        }

        #endregion

        // 双子座
        #region
        private double _geminiX;
        public double geminiX
        {
            get
            {
                return _geminiX;
            }
            set
            {
                _geminiX = value;
                OnPropertyChanged("geminix");
            }
        }

        private double _geminiY;
        public double geminiY
        {
            get
            {
                return _geminiY;
            }
            set
            {
                _geminiY = value;
                OnPropertyChanged("geminiy");
            }
        }
        private string _geminiTxt;
        public string geminiTxt
        {
            get
            {
                return _geminiTxt;
            }
            set
            {
                _geminiTxt = value;
                OnPropertyChanged("geminiTxt");
            }
        }


        #endregion


        // 蟹座
        #region
        private double _cancerX;
        public double cancerX
        {
            get
            {
                return _cancerX;
            }
            set
            {
                _cancerX = value;
                OnPropertyChanged("cancerx");
            }
        }

        private double _cancerY;
        public double cancerY
        {
            get
            {
                return _cancerY;
            }
            set
            {
                _cancerY = value;
                OnPropertyChanged("cancery");
            }
        }

        private string _cancerTxt;
        public string cancerTxt
        {
            get
            {
                return _cancerTxt;
            }
            set
            {
                _cancerTxt = value;
                OnPropertyChanged("cancerTxt");
            }
        }

        #endregion

        // 獅子座
        #region
        private double _leoX;
        public double leoX
        {
            get
            {
                return _leoX;
            }
            set
            {
                _leoX = value;
                OnPropertyChanged("leox");
            }
        }

        private double _leoY;
        public double leoY
        {
            get
            {
                return _leoY;
            }
            set
            {
                _leoY = value;
                OnPropertyChanged("leoy");
            }
        }

        private string _leoTxt;
        public string leoTxt
        {
            get
            {
                return _leoTxt;
            }
            set
            {
                _leoTxt = value;
                OnPropertyChanged("leoTxt");
            }
        }

        #endregion

        // 乙女座
        #region
        private double _virgoX;
        public double virgoX
        {
            get
            {
                return _virgoX;
            }
            set
            {
                _virgoX = value;
                OnPropertyChanged("virgox");
            }
        }

        private double _virgoY;
        public double virgoY
        {
            get
            {
                return _virgoY;
            }
            set
            {
                _virgoY = value;
                OnPropertyChanged("virgoy");
            }
        }

        private string _virgoTxt;
        public string virgoTxt
        {
            get
            {
                return _virgoTxt;
            }
            set
            {
                _virgoTxt = value;
                OnPropertyChanged("virgoTxt");
            }
        }

        #endregion

        // 天秤座
        #region
        private double _libraX;
        public double libraX
        {
            get
            {
                return _libraX;
            }
            set
            {
                _libraX = value;
                OnPropertyChanged("librax");
            }
        }

        private double _libraY;
        public double libraY
        {
            get
            {
                return _libraY;
            }
            set
            {
                _libraY = value;
                OnPropertyChanged("libray");
            }
        }

        private string _libraTxt;
        public string libraTxt
        {
            get
            {
                return _libraTxt;
            }
            set
            {
                _libraTxt = value;
                OnPropertyChanged("libraTxt");
            }
        }

        #endregion

        // 蠍座
        #region
        private double _scorpionX;
        public double scorpionX
        {
            get
            {
                return _scorpionX;
            }
            set
            {
                _scorpionX = value;
                OnPropertyChanged("scorpionx");
            }
        }

        private double _scorpionY;
        public double scorpionY
        {
            get
            {
                return _scorpionY;
            }
            set
            {
                _scorpionY = value;
                OnPropertyChanged("scorpiony");
            }
        }

        private string _scorpionTxt;
        public string scorpionTxt
        {
            get
            {
                return _scorpionTxt;
            }
            set
            {
                _scorpionTxt = value;
                OnPropertyChanged("scorpionTxt");
            }
        }
        #endregion


        // 射手座
        #region
        private double _sagittariusX;
        public double sagittariusX
        {
            get
            {
                return _sagittariusX;
            }
            set
            {
                _sagittariusX = value;
                OnPropertyChanged("sagittariusx");
            }
        }

        private double _sagittariusY;
        public double sagittariusY
        {
            get
            {
                return _sagittariusY;
            }
            set
            {
                _sagittariusY = value;
                OnPropertyChanged("sagittariusy");
            }
        }

        private string _sagittariusTxt;
        public string sagittariusTxt
        {
            get
            {
                return _sagittariusTxt;
            }
            set
            {
                _sagittariusTxt = value;
                OnPropertyChanged("sagittariusTxt");
            }
        }
        #endregion

        // 山羊座
        #region
        private double _capricornX;
        public double capricornX
        {
            get
            {
                return _capricornX;
            }
            set
            {
                _capricornX = value;
                OnPropertyChanged("capricornx");
            }
        }

        private double _capricornY;
        public double capricornY
        {
            get
            {
                return _capricornY;
            }
            set
            {
                _capricornY = value;
                OnPropertyChanged("capricorny");
            }
        }
        private string _capricornTxt;
        public string capricornTxt
        {
            get
            {
                return _capricornTxt;
            }
            set
            {
                _capricornTxt = value;
                OnPropertyChanged("capricornTxt");
            }
        }

        #endregion

        // 水瓶座
        #region
        private double _aquariusX;
        public double aquariusX
        {
            get
            {
                return _aquariusX;
            }
            set
            {
                _aquariusX = value;
                OnPropertyChanged("aquariusx");
            }
        }

        private double _aquariusY;
        public double aquariusY
        {
            get
            {
                return _aquariusY;
            }
            set
            {
                _aquariusY = value;
                OnPropertyChanged("aquariusy");
            }
        }
        private string _aquariusTxt;
        public string aquariusTxt
        {
            get
            {
                return _aquariusTxt;
            }
            set
            {
                _aquariusTxt = value;
                OnPropertyChanged("aquariusTxt");
            }
        }

        #endregion

        // 魚座
        #region
        private double _piscesX;
        public double piscesX
        {
            get
            {
                return _piscesX;
            }
            set
            {
                _piscesX = value;
                OnPropertyChanged("piscesx");
            }
        }

        private double _piscesY;
        public double piscesY
        {
            get
            {
                return _piscesY;
            }
            set
            {
                _piscesY = value;
                OnPropertyChanged("piscesy");
            }
        }
        private string _piscesTxt;
        public string piscesTxt
        {
            get
            {
                return _piscesTxt;
            }
            set
            {
                _piscesTxt = value;
                OnPropertyChanged("piscesTxt");
            }
        }

        #endregion

        // 太陽
        #region
        private double _natalSunX;
        public double natalSunX
        {
            get
            {
                return _natalSunX;
            }
            set
            {
                _natalSunX = value;
                OnPropertyChanged("natalSunx");
            }
        }

        private double _natalSunY;
        public double natalSunY
        {
            get
            {
                return _natalSunY;
            }
            set
            {
                _natalSunY = value;
                OnPropertyChanged("natalSuny");
            }
        }
        private string _natalSunTxt;
        public string natalSunTxt
        {
            get
            {
                return _natalSunTxt;
            }
            set
            {
                _natalSunTxt = value;
                OnPropertyChanged("natalSunTxt");
            }
        }
        private double _natalSunDegreeX;
        public double natalSunDegreeX
        {
            get
            {
                return _natalSunDegreeX;
            }
            set
            {
                _natalSunDegreeX = value;
                OnPropertyChanged("natalSunDegreeX");
            }
        }

        private double _natalSunDegreeY;
        public double natalSunDegreeY
        {
            get
            {
                return _natalSunDegreeY;
            }
            set
            {
                _natalSunDegreeY = value;
                OnPropertyChanged("natalSunDegreeY");
            }
        }
        private string _natalSunDegreeTxt;
        public string natalSunDegreeTxt
        {
            get
            {
                return _natalSunDegreeTxt;
            }
            set
            {
                _natalSunDegreeTxt = value;
                OnPropertyChanged("natalSunDegreeTxt");
            }
        }
        private double _natalSunSignX;
        public double natalSunSignX
        {
            get
            {
                return _natalSunSignX;
            }
            set
            {
                _natalSunSignX = value;
                OnPropertyChanged("natalSunSignX");
            }
        }

        private double _natalSunSignY;
        public double natalSunSignY
        {
            get
            {
                return _natalSunSignY;
            }
            set
            {
                _natalSunSignY = value;
                OnPropertyChanged("natalSunSignY");
            }
        }
        private string _natalSunSignTxt;
        public string natalSunSignTxt
        {
            get
            {
                return _natalSunSignTxt;
            }
            set
            {
                _natalSunSignTxt = value;
                OnPropertyChanged("natalSunSignTxt");
            }
        }
        private double _natalSunMinuteX;
        public double natalSunMinuteX
        {
            get
            {
                return _natalSunMinuteX;
            }
            set
            {
                _natalSunMinuteX = value;
                OnPropertyChanged("natalSunMinuteX");
            }
        }

        private double _natalSunMinuteY;
        public double natalSunMinuteY
        {
            get
            {
                return _natalSunMinuteY;
            }
            set
            {
                _natalSunMinuteY = value;
                OnPropertyChanged("natalSunMinuteY");
            }
        }
        private string _natalSunMinuteTxt;
        public string natalSunMinuteTxt
        {
            get
            {
                return _natalSunMinuteTxt;
            }
            set
            {
                _natalSunMinuteTxt = value;
                OnPropertyChanged("natalSunMinuteTxt");
            }
        }
        private double _natalSunRetrogradeX;
        public double natalSunRetrogradeX
        {
            get
            {
                return _natalSunRetrogradeX;
            }
            set
            {
                _natalSunRetrogradeX = value;
                OnPropertyChanged("natalSunRetrogradeX");
            }
        }

        private double _natalSunRetrogradeY;
        public double natalSunRetrogradeY
        {
            get
            {
                return _natalSunRetrogradeY;
            }
            set
            {
                _natalSunRetrogradeY = value;
                OnPropertyChanged("natalSunRetrogradeY");
            }
        }
        private string _natalSunRetrogradeTxt;
        public string natalSunRetrogradeTxt
        {
            get
            {
                return _natalSunRetrogradeTxt;
            }
            set
            {
                _natalSunRetrogradeTxt = value;
                OnPropertyChanged("natalSunRetrogradeTxt");
            }
        }

        private double _natalSunAngle;
        public double natalSunAngle
        {
            get
            {
                return _natalSunAngle;
            }
            set
            {
                _natalSunAngle = value;
                OnPropertyChanged("natalSunAngle");
            }
        }

        #endregion

        // 月
        #region
        private double _natalMoonX;
        public double natalMoonX
        {
            get
            {
                return _natalMoonX;
            }
            set
            {
                _natalMoonX = value;
                OnPropertyChanged("natalMoonX");
            }
        }

        private double _natalMoonY;
        public double natalMoonY
        {
            get
            {
                return _natalMoonY;
            }
            set
            {
                _natalMoonY = value;
                OnPropertyChanged("natalMoonY");
            }
        }
        private string _natalMoonTxt;
        public string natalMoonTxt
        {
            get
            {
                return _natalMoonTxt;
            }
            set
            {
                _natalMoonTxt = value;
                OnPropertyChanged("natalMoonTxt");
            }
        }
        private double _natalMoonDegreeX;
        public double natalMoonDegreeX
        {
            get
            {
                return _natalMoonDegreeX;
            }
            set
            {
                _natalMoonDegreeX = value;
                OnPropertyChanged("natalMoonDegreeX");
            }
        }

        private double _natalMoonDegreeY;
        public double natalMoonDegreeY
        {
            get
            {
                return _natalMoonDegreeY;
            }
            set
            {
                _natalMoonDegreeY = value;
                OnPropertyChanged("natalMoonDegreeY");
            }
        }
        private string _natalMoonDegreeTxt;
        public string natalMoonDegreeTxt
        {
            get
            {
                return _natalMoonDegreeTxt;
            }
            set
            {
                _natalMoonDegreeTxt = value;
                OnPropertyChanged("natalMoonDegreeTxt");
            }
        }
        private double _natalMoonSignX;
        public double natalMoonSignX
        {
            get
            {
                return _natalMoonSignX;
            }
            set
            {
                _natalMoonSignX = value;
                OnPropertyChanged("natalMoonSignX");
            }
        }

        private double _natalMoonSignY;
        public double natalMoonSignY
        {
            get
            {
                return _natalMoonSignY;
            }
            set
            {
                _natalMoonSignY = value;
                OnPropertyChanged("natalMoonSignY");
            }
        }
        private string _natalMoonSignTxt;
        public string natalMoonSignTxt
        {
            get
            {
                return _natalMoonSignTxt;
            }
            set
            {
                _natalMoonSignTxt = value;
                OnPropertyChanged("natalMoonSignTxt");
            }
        }
        private double _natalMoonMinuteX;
        public double natalMoonMinuteX
        {
            get
            {
                return _natalMoonMinuteX;
            }
            set
            {
                _natalMoonMinuteX = value;
                OnPropertyChanged("natalMoonMinuteX");
            }
        }

        private double _natalMoonMinuteY;
        public double natalMoonMinuteY
        {
            get
            {
                return _natalMoonMinuteY;
            }
            set
            {
                _natalMoonMinuteY = value;
                OnPropertyChanged("natalMoonMinuteY");
            }
        }
        private string _natalMoonMinuteTxt;
        public string natalMoonMinuteTxt
        {
            get
            {
                return _natalMoonMinuteTxt;
            }
            set
            {
                _natalMoonMinuteTxt = value;
                OnPropertyChanged("natalMoonMinuteTxt");
            }
        }
        private double _natalMoonRetrogradeX;
        public double natalMoonRetrogradeX
        {
            get
            {
                return _natalMoonRetrogradeX;
            }
            set
            {
                _natalMoonRetrogradeX = value;
                OnPropertyChanged("natalMoonRetrogradeX");
            }
        }

        private double _natalMoonRetrogradeY;
        public double natalMoonRetrogradeY
        {
            get
            {
                return _natalMoonRetrogradeY;
            }
            set
            {
                _natalMoonRetrogradeY = value;
                OnPropertyChanged("natalMoonRetrogradeY");
            }
        }
        private string _natalMoonRetrogradeTxt;
        public string natalMoonRetrogradeTxt
        {
            get
            {
                return _natalMoonRetrogradeTxt;
            }
            set
            {
                _natalMoonRetrogradeTxt = value;
                OnPropertyChanged("natalMoonRetrogradeTxt");
            }
        }
        private double _natalMoonAngle;
        public double natalMoonAngle
        {
            get
            {
                return _natalMoonAngle;
            }
            set
            {
                _natalMoonAngle = value;
                OnPropertyChanged("natalMoonAngle");
            }
        }

        #endregion

        // 水星
        #region
        private double _natalMercuryX;
        public double natalMercuryX
        {
            get
            {
                return _natalMercuryX;
            }
            set
            {
                _natalMercuryX = value;
                OnPropertyChanged("natalMercuryx");
            }
        }

        private double _natalMercuryY;
        public double natalMercuryY
        {
            get
            {
                return _natalMercuryY;
            }
            set
            {
                _natalMercuryY = value;
                OnPropertyChanged("natalMercuryy");
            }
        }
        private string _natalMercuryTxt;
        public string natalMercuryTxt
        {
            get
            {
                return _natalMercuryTxt;
            }
            set
            {
                _natalMercuryTxt = value;
                OnPropertyChanged("natalMercuryTxt");
            }
        }
        private double _natalMercuryDegreeX;
        public double natalMercuryDegreeX
        {
            get
            {
                return _natalMercuryDegreeX;
            }
            set
            {
                _natalMercuryDegreeX = value;
                OnPropertyChanged("natalMercuryDegreeX");
            }
        }

        private double _natalMercuryDegreeY;
        public double natalMercuryDegreeY
        {
            get
            {
                return _natalMercuryDegreeY;
            }
            set
            {
                _natalMercuryDegreeY = value;
                OnPropertyChanged("natalMercuryDegreeY");
            }
        }
        private string _natalMercuryDegreeTxt;
        public string natalMercuryDegreeTxt
        {
            get
            {
                return _natalMercuryDegreeTxt;
            }
            set
            {
                _natalMercuryDegreeTxt = value;
                OnPropertyChanged("natalMercuryDegreeTxt");
            }
        }
        private double _natalMercurySignX;
        public double natalMercurySignX
        {
            get
            {
                return _natalMercurySignX;
            }
            set
            {
                _natalMercurySignX = value;
                OnPropertyChanged("natalMercurySignX");
            }
        }

        private double _natalMercurySignY;
        public double natalMercurySignY
        {
            get
            {
                return _natalMercurySignY;
            }
            set
            {
                _natalMercurySignY = value;
                OnPropertyChanged("natalMercurySignY");
            }
        }
        private string _natalMercurySignTxt;
        public string natalMercurySignTxt
        {
            get
            {
                return _natalMercurySignTxt;
            }
            set
            {
                _natalMercurySignTxt = value;
                OnPropertyChanged("natalMercurySignTxt");
            }
        }
        private double _natalMercuryMinuteX;
        public double natalMercuryMinuteX
        {
            get
            {
                return _natalMercuryMinuteX;
            }
            set
            {
                _natalMercuryMinuteX = value;
                OnPropertyChanged("natalMercuryMinuteX");
            }
        }

        private double _natalMercuryMinuteY;
        public double natalMercuryMinuteY
        {
            get
            {
                return _natalMercuryMinuteY;
            }
            set
            {
                _natalMercuryMinuteY = value;
                OnPropertyChanged("natalMercuryMinuteY");
            }
        }
        private string _natalMercuryMinuteTxt;
        public string natalMercuryMinuteTxt
        {
            get
            {
                return _natalMercuryMinuteTxt;
            }
            set
            {
                _natalMercuryMinuteTxt = value;
                OnPropertyChanged("natalMercuryMinuteTxt");
            }
        }
        private double _natalMercuryRetrogradeX;
        public double natalMercuryRetrogradeX
        {
            get
            {
                return _natalMercuryRetrogradeX;
            }
            set
            {
                _natalMercuryRetrogradeX = value;
                OnPropertyChanged("natalMercuryRetrogradeX");
            }
        }

        private double _natalMercuryRetrogradeY;
        public double natalMercuryRetrogradeY
        {
            get
            {
                return _natalMercuryRetrogradeY;
            }
            set
            {
                _natalMercuryRetrogradeY = value;
                OnPropertyChanged("natalMercuryRetrogradeY");
            }
        }
        private string _natalMercuryRetrogradeTxt;
        public string natalMercuryRetrogradeTxt
        {
            get
            {
                return _natalMercuryRetrogradeTxt;
            }
            set
            {
                _natalMercuryRetrogradeTxt = value;
                OnPropertyChanged("natalMercuryRetrogradeTxt");
            }
        }
        private double _natalMercuryAngle;
        public double natalMercuryAngle
        {
            get
            {
                return _natalMercuryAngle;
            }
            set
            {
                _natalMercuryAngle = value;
                OnPropertyChanged("natalMercuryAngle");
            }
        }

        #endregion

        // 金星
        #region
        private double _natalVenusX;
        public double natalVenusX
        {
            get
            {
                return _natalVenusX;
            }
            set
            {
                _natalVenusX = value;
                OnPropertyChanged("natalVenusX");
            }
        }

        private double _natalVenusY;
        public double natalVenusY
        {
            get
            {
                return _natalVenusY;
            }
            set
            {
                _natalVenusY = value;
                OnPropertyChanged("natalVenusY");
            }
        }
        private string _natalVenusTxt;
        public string natalVenusTxt
        {
            get
            {
                return _natalVenusTxt;
            }
            set
            {
                _natalVenusTxt = value;
                OnPropertyChanged("natalVenusTxt");
            }
        }
        private double _natalVenusDegreeX;
        public double natalVenusDegreeX
        {
            get
            {
                return _natalVenusDegreeX;
            }
            set
            {
                _natalVenusDegreeX = value;
                OnPropertyChanged("natalVenusDegreeX");
            }
        }

        private double _natalVenusDegreeY;
        public double natalVenusDegreeY
        {
            get
            {
                return _natalVenusDegreeY;
            }
            set
            {
                _natalVenusDegreeY = value;
                OnPropertyChanged("natalVenusDegreeY");
            }
        }
        private string _natalVenusDegreeTxt;
        public string natalVenusDegreeTxt
        {
            get
            {
                return _natalVenusDegreeTxt;
            }
            set
            {
                _natalVenusDegreeTxt = value;
                OnPropertyChanged("natalVenusDegreeTxt");
            }
        }
        private double _natalVenusSignX;
        public double natalVenusSignX
        {
            get
            {
                return _natalVenusSignX;
            }
            set
            {
                _natalVenusSignX = value;
                OnPropertyChanged("natalVenusSignX");
            }
        }

        private double _natalVenusSignY;
        public double natalVenusSignY
        {
            get
            {
                return _natalVenusSignY;
            }
            set
            {
                _natalVenusSignY = value;
                OnPropertyChanged("natalVenusSignY");
            }
        }
        private string _natalVenusSignTxt;
        public string natalVenusSignTxt
        {
            get
            {
                return _natalVenusSignTxt;
            }
            set
            {
                _natalVenusSignTxt = value;
                OnPropertyChanged("natalVenusSignTxt");
            }
        }
        private double _natalVenusMinuteX;
        public double natalVenusMinuteX
        {
            get
            {
                return _natalVenusMinuteX;
            }
            set
            {
                _natalVenusMinuteX = value;
                OnPropertyChanged("natalVenusMinuteX");
            }
        }

        private double _natalVenusMinuteY;
        public double natalVenusMinuteY
        {
            get
            {
                return _natalVenusMinuteY;
            }
            set
            {
                _natalVenusMinuteY = value;
                OnPropertyChanged("natalVenusMinuteY");
            }
        }
        private string _natalVenusMinuteTxt;
        public string natalVenusMinuteTxt
        {
            get
            {
                return _natalVenusMinuteTxt;
            }
            set
            {
                _natalVenusMinuteTxt = value;
                OnPropertyChanged("natalVenusMinuteTxt");
            }
        }
        private double _natalVenusRetrogradeX;
        public double natalVenusRetrogradeX
        {
            get
            {
                return _natalVenusRetrogradeX;
            }
            set
            {
                _natalVenusRetrogradeX = value;
                OnPropertyChanged("natalVenusRetrogradeX");
            }
        }

        private double _natalVenusRetrogradeY;
        public double natalVenusRetrogradeY
        {
            get
            {
                return _natalVenusRetrogradeY;
            }
            set
            {
                _natalVenusRetrogradeY = value;
                OnPropertyChanged("natalVenusRetrogradeY");
            }
        }
        private string _natalVenusRetrogradeTxt;
        public string natalVenusRetrogradeTxt
        {
            get
            {
                return _natalVenusRetrogradeTxt;
            }
            set
            {
                _natalVenusRetrogradeTxt = value;
                OnPropertyChanged("natalVenusRetrogradeTxt");
            }
        }
        private double _natalVenusAngle;
        public double natalVenusAngle
        {
            get
            {
                return _natalVenusAngle;
            }
            set
            {
                _natalVenusAngle = value;
                OnPropertyChanged("natalVenusAngle");
            }
        }

        #endregion

        // 火星
        #region
        private double _natalMarsX;
        public double natalMarsX
        {
            get
            {
                return _natalMarsX;
            }
            set
            {
                _natalMarsX = value;
                OnPropertyChanged("natalMarsx");
            }
        }

        private double _natalMarsY;
        public double natalMarsY
        {
            get
            {
                return _natalMarsY;
            }
            set
            {
                _natalMarsY = value;
                OnPropertyChanged("natalMarsy");
            }
        }
        private string _natalMarsTxt;
        public string natalMarsTxt
        {
            get
            {
                return _natalMarsTxt;
            }
            set
            {
                _natalMarsTxt = value;
                OnPropertyChanged("natalMarsTxt");
            }
        }
        private double _natalMarsDegreeX;
        public double natalMarsDegreeX
        {
            get
            {
                return _natalMarsDegreeX;
            }
            set
            {
                _natalMarsDegreeX = value;
                OnPropertyChanged("natalMarsDegreeX");
            }
        }

        private double _natalMarsDegreeY;
        public double natalMarsDegreeY
        {
            get
            {
                return _natalMarsDegreeY;
            }
            set
            {
                _natalMarsDegreeY = value;
                OnPropertyChanged("natalMarsDegreeY");
            }
        }
        private string _natalMarsDegreeTxt;
        public string natalMarsDegreeTxt
        {
            get
            {
                return _natalMarsDegreeTxt;
            }
            set
            {
                _natalMarsDegreeTxt = value;
                OnPropertyChanged("natalMarsDegreeTxt");
            }
        }
        private double _natalMarsSignX;
        public double natalMarsSignX
        {
            get
            {
                return _natalMarsSignX;
            }
            set
            {
                _natalMarsSignX = value;
                OnPropertyChanged("natalMarsSignX");
            }
        }

        private double _natalMarsSignY;
        public double natalMarsSignY
        {
            get
            {
                return _natalMarsSignY;
            }
            set
            {
                _natalMarsSignY = value;
                OnPropertyChanged("natalMarsSignY");
            }
        }
        private string _natalMarsSignTxt;
        public string natalMarsSignTxt
        {
            get
            {
                return _natalMarsSignTxt;
            }
            set
            {
                _natalMarsSignTxt = value;
                OnPropertyChanged("natalMarsSignTxt");
            }
        }
        private double _natalMarsMinuteX;
        public double natalMarsMinuteX
        {
            get
            {
                return _natalMarsMinuteX;
            }
            set
            {
                _natalMarsMinuteX = value;
                OnPropertyChanged("natalMarsMinuteX");
            }
        }

        private double _natalMarsMinuteY;
        public double natalMarsMinuteY
        {
            get
            {
                return _natalMarsMinuteY;
            }
            set
            {
                _natalMarsMinuteY = value;
                OnPropertyChanged("natalMarsMinuteY");
            }
        }
        private string _natalMarsMinuteTxt;
        public string natalMarsMinuteTxt
        {
            get
            {
                return _natalMarsMinuteTxt;
            }
            set
            {
                _natalMarsMinuteTxt = value;
                OnPropertyChanged("natalMarsMinuteTxt");
            }
        }
        private double _natalMarsRetrogradeX;
        public double natalMarsRetrogradeX
        {
            get
            {
                return _natalMarsRetrogradeX;
            }
            set
            {
                _natalMarsRetrogradeX = value;
                OnPropertyChanged("natalMarsRetrogradeX");
            }
        }

        private double _natalMarsRetrogradeY;
        public double natalMarsRetrogradeY
        {
            get
            {
                return _natalMarsRetrogradeY;
            }
            set
            {
                _natalMarsRetrogradeY = value;
                OnPropertyChanged("natalMarsRetrogradeY");
            }
        }
        private string _natalMarsRetrogradeTxt;
        public string natalMarsRetrogradeTxt
        {
            get
            {
                return _natalMarsRetrogradeTxt;
            }
            set
            {
                _natalMarsRetrogradeTxt = value;
                OnPropertyChanged("natalMarsRetrogradeTxt");
            }
        }
        private double _natalMarsAngle;
        public double natalMarsAngle
        {
            get
            {
                return _natalMarsAngle;
            }
            set
            {
                _natalMarsAngle = value;
                OnPropertyChanged("natalMarsAngle");
            }
        }

        #endregion

        // 木星
        #region
        private double _natalJupiterX;
        public double natalJupiterX
        {
            get
            {
                return _natalJupiterX;
            }
            set
            {
                _natalJupiterX = value;
                OnPropertyChanged("natalJupiterX");
            }
        }

        private double _natalJupiterY;
        public double natalJupiterY
        {
            get
            {
                return _natalJupiterY;
            }
            set
            {
                _natalJupiterY = value;
                OnPropertyChanged("natalJupiterY");
            }
        }
        private string _natalJupiterTxt;
        public string natalJupiterTxt
        {
            get
            {
                return _natalJupiterTxt;
            }
            set
            {
                _natalJupiterTxt = value;
                OnPropertyChanged("natalJupiterTxt");
            }
        }
        private double _natalJupiterDegreeX;
        public double natalJupiterDegreeX
        {
            get
            {
                return _natalJupiterDegreeX;
            }
            set
            {
                _natalJupiterDegreeX = value;
                OnPropertyChanged("natalJupiterDegreeX");
            }
        }

        private double _natalJupiterDegreeY;
        public double natalJupiterDegreeY
        {
            get
            {
                return _natalJupiterDegreeY;
            }
            set
            {
                _natalJupiterDegreeY = value;
                OnPropertyChanged("natalJupiterDegreeY");
            }
        }
        private string _natalJupiterDegreeTxt;
        public string natalJupiterDegreeTxt
        {
            get
            {
                return _natalJupiterDegreeTxt;
            }
            set
            {
                _natalJupiterDegreeTxt = value;
                OnPropertyChanged("natalJupiterDegreeTxt");
            }
        }
        private double _natalJupiterSignX;
        public double natalJupiterSignX
        {
            get
            {
                return _natalJupiterSignX;
            }
            set
            {
                _natalJupiterSignX = value;
                OnPropertyChanged("natalJupiterSignX");
            }
        }

        private double _natalJupiterSignY;
        public double natalJupiterSignY
        {
            get
            {
                return _natalJupiterSignY;
            }
            set
            {
                _natalJupiterSignY = value;
                OnPropertyChanged("natalJupiterSignY");
            }
        }
        private string _natalJupiterSignTxt;
        public string natalJupiterSignTxt
        {
            get
            {
                return _natalJupiterSignTxt;
            }
            set
            {
                _natalJupiterSignTxt = value;
                OnPropertyChanged("natalJupiterSignTxt");
            }
        }
        private double _natalJupiterMinuteX;
        public double natalJupiterMinuteX
        {
            get
            {
                return _natalJupiterMinuteX;
            }
            set
            {
                _natalJupiterMinuteX = value;
                OnPropertyChanged("natalJupiterMinuteX");
            }
        }

        private double _natalJupiterMinuteY;
        public double natalJupiterMinuteY
        {
            get
            {
                return _natalJupiterMinuteY;
            }
            set
            {
                _natalJupiterMinuteY = value;
                OnPropertyChanged("natalJupiterMinuteY");
            }
        }
        private string _natalJupiterMinuteTxt;
        public string natalJupiterMinuteTxt
        {
            get
            {
                return _natalJupiterMinuteTxt;
            }
            set
            {
                _natalJupiterMinuteTxt = value;
                OnPropertyChanged("natalJupiterMinuteTxt");
            }
        }
        private double _natalJupiterRetrogradeX;
        public double natalJupiterRetrogradeX
        {
            get
            {
                return _natalJupiterRetrogradeX;
            }
            set
            {
                _natalJupiterRetrogradeX = value;
                OnPropertyChanged("natalJupiterRetrogradeX");
            }
        }

        private double _natalJupiterRetrogradeY;
        public double natalJupiterRetrogradeY
        {
            get
            {
                return _natalJupiterRetrogradeY;
            }
            set
            {
                _natalJupiterRetrogradeY = value;
                OnPropertyChanged("natalJupiterRetrogradeY");
            }
        }
        private string _natalJupiterRetrogradeTxt;
        public string natalJupiterRetrogradeTxt
        {
            get
            {
                return _natalJupiterRetrogradeTxt;
            }
            set
            {
                _natalJupiterRetrogradeTxt = value;
                OnPropertyChanged("natalJupiterRetrogradeTxt");
            }
        }
        private double _natalJupiterAngle;
        public double natalJupiterAngle
        {
            get
            {
                return _natalJupiterAngle;
            }
            set
            {
                _natalJupiterAngle = value;
                OnPropertyChanged("natalJupiterAngle");
            }
        }

        #endregion

        // 土星
        #region
        private double _natalSaturnX;
        public double natalSaturnX
        {
            get
            {
                return _natalSaturnX;
            }
            set
            {
                _natalSaturnX = value;
                OnPropertyChanged("natalSaturnx");
            }
        }

        private double _natalSaturnY;
        public double natalSaturnY
        {
            get
            {
                return _natalSaturnY;
            }
            set
            {
                _natalSaturnY = value;
                OnPropertyChanged("natalSaturny");
            }
        }
        private string _natalSaturnTxt;
        public string natalSaturnTxt
        {
            get
            {
                return _natalSaturnTxt;
            }
            set
            {
                _natalSaturnTxt = value;
                OnPropertyChanged("natalSaturnTxt");
            }
        }
        private double _natalSaturnDegreeX;
        public double natalSaturnDegreeX
        {
            get
            {
                return _natalSaturnDegreeX;
            }
            set
            {
                _natalSaturnDegreeX = value;
                OnPropertyChanged("natalSaturnDegreeX");
            }
        }

        private double _natalSaturnDegreeY;
        public double natalSaturnDegreeY
        {
            get
            {
                return _natalSaturnDegreeY;
            }
            set
            {
                _natalSaturnDegreeY = value;
                OnPropertyChanged("natalSaturnDegreeY");
            }
        }
        private string _natalSaturnDegreeTxt;
        public string natalSaturnDegreeTxt
        {
            get
            {
                return _natalSaturnDegreeTxt;
            }
            set
            {
                _natalSaturnDegreeTxt = value;
                OnPropertyChanged("natalSaturnDegreeTxt");
            }
        }
        private double _natalSaturnSignX;
        public double natalSaturnSignX
        {
            get
            {
                return _natalSaturnSignX;
            }
            set
            {
                _natalSaturnSignX = value;
                OnPropertyChanged("natalSaturnSignX");
            }
        }

        private double _natalSaturnSignY;
        public double natalSaturnSignY
        {
            get
            {
                return _natalSaturnSignY;
            }
            set
            {
                _natalSaturnSignY = value;
                OnPropertyChanged("natalSaturnSignY");
            }
        }
        private string _natalSaturnSignTxt;
        public string natalSaturnSignTxt
        {
            get
            {
                return _natalSaturnSignTxt;
            }
            set
            {
                _natalSaturnSignTxt = value;
                OnPropertyChanged("natalSaturnSignTxt");
            }
        }
        private double _natalSaturnMinuteX;
        public double natalSaturnMinuteX
        {
            get
            {
                return _natalSaturnMinuteX;
            }
            set
            {
                _natalSaturnMinuteX = value;
                OnPropertyChanged("natalSaturnMinuteX");
            }
        }

        private double _natalSaturnMinuteY;
        public double natalSaturnMinuteY
        {
            get
            {
                return _natalSaturnMinuteY;
            }
            set
            {
                _natalSaturnMinuteY = value;
                OnPropertyChanged("natalSaturnMinuteY");
            }
        }
        private string _natalSaturnMinuteTxt;
        public string natalSaturnMinuteTxt
        {
            get
            {
                return _natalSaturnMinuteTxt;
            }
            set
            {
                _natalSaturnMinuteTxt = value;
                OnPropertyChanged("natalSaturnMinuteTxt");
            }
        }
        private double _natalSaturnRetrogradeX;
        public double natalSaturnRetrogradeX
        {
            get
            {
                return _natalSaturnRetrogradeX;
            }
            set
            {
                _natalSaturnRetrogradeX = value;
                OnPropertyChanged("natalSaturnRetrogradeX");
            }
        }

        private double _natalSaturnRetrogradeY;
        public double natalSaturnRetrogradeY
        {
            get
            {
                return _natalSaturnRetrogradeY;
            }
            set
            {
                _natalSaturnRetrogradeY = value;
                OnPropertyChanged("natalSaturnRetrogradeY");
            }
        }
        private string _natalSaturnRetrogradeTxt;
        public string natalSaturnRetrogradeTxt
        {
            get
            {
                return _natalSaturnRetrogradeTxt;
            }
            set
            {
                _natalSaturnRetrogradeTxt = value;
                OnPropertyChanged("natalSaturnRetrogradeTxt");
            }
        }
        private double _natalSaturnAngle;
        public double natalSaturnAngle
        {
            get
            {
                return _natalSaturnAngle;
            }
            set
            {
                _natalSaturnAngle = value;
                OnPropertyChanged("natalSaturnAngle");
            }
        }

        #endregion

        // 天王星
        #region
        private double _natalUranusX;
        public double natalUranusX
        {
            get
            {
                return _natalUranusX;
            }
            set
            {
                _natalUranusX = value;
                OnPropertyChanged("natalUranusX");
            }
        }

        private double _natalUranusY;
        public double natalUranusY
        {
            get
            {
                return _natalUranusY;
            }
            set
            {
                _natalUranusY = value;
                OnPropertyChanged("natalUranusY");
            }
        }
        private string _natalUranusTxt;
        public string natalUranusTxt
        {
            get
            {
                return _natalUranusTxt;
            }
            set
            {
                _natalUranusTxt = value;
                OnPropertyChanged("natalUranusTxt");
            }
        }
        private double _natalUranusDegreeX;
        public double natalUranusDegreeX
        {
            get
            {
                return _natalUranusDegreeX;
            }
            set
            {
                _natalUranusDegreeX = value;
                OnPropertyChanged("natalUranusDegreeX");
            }
        }

        private double _natalUranusDegreeY;
        public double natalUranusDegreeY
        {
            get
            {
                return _natalUranusDegreeY;
            }
            set
            {
                _natalUranusDegreeY = value;
                OnPropertyChanged("natalUranusDegreeY");
            }
        }
        private string _natalUranusDegreeTxt;
        public string natalUranusDegreeTxt
        {
            get
            {
                return _natalUranusDegreeTxt;
            }
            set
            {
                _natalUranusDegreeTxt = value;
                OnPropertyChanged("natalUranusDegreeTxt");
            }
        }
        private double _natalUranusSignX;
        public double natalUranusSignX
        {
            get
            {
                return _natalUranusSignX;
            }
            set
            {
                _natalUranusSignX = value;
                OnPropertyChanged("natalUranusSignX");
            }
        }

        private double _natalUranusSignY;
        public double natalUranusSignY
        {
            get
            {
                return _natalUranusSignY;
            }
            set
            {
                _natalUranusSignY = value;
                OnPropertyChanged("natalUranusSignY");
            }
        }
        private string _natalUranusSignTxt;
        public string natalUranusSignTxt
        {
            get
            {
                return _natalUranusSignTxt;
            }
            set
            {
                _natalUranusSignTxt = value;
                OnPropertyChanged("natalUranusSignTxt");
            }
        }
        private double _natalUranusMinuteX;
        public double natalUranusMinuteX
        {
            get
            {
                return _natalUranusMinuteX;
            }
            set
            {
                _natalUranusMinuteX = value;
                OnPropertyChanged("natalUranusMinuteX");
            }
        }

        private double _natalUranusMinuteY;
        public double natalUranusMinuteY
        {
            get
            {
                return _natalUranusMinuteY;
            }
            set
            {
                _natalUranusMinuteY = value;
                OnPropertyChanged("natalUranusMinuteY");
            }
        }
        private string _natalUranusMinuteTxt;
        public string natalUranusMinuteTxt
        {
            get
            {
                return _natalUranusMinuteTxt;
            }
            set
            {
                _natalUranusMinuteTxt = value;
                OnPropertyChanged("natalUranusMinuteTxt");
            }
        }
        private double _natalUranusRetrogradeX;
        public double natalUranusRetrogradeX
        {
            get
            {
                return _natalUranusRetrogradeX;
            }
            set
            {
                _natalUranusRetrogradeX = value;
                OnPropertyChanged("natalUranusRetrogradeX");
            }
        }

        private double _natalUranusRetrogradeY;
        public double natalUranusRetrogradeY
        {
            get
            {
                return _natalUranusRetrogradeY;
            }
            set
            {
                _natalUranusRetrogradeY = value;
                OnPropertyChanged("natalUranusRetrogradeY");
            }
        }
        private string _natalUranusRetrogradeTxt;
        public string natalUranusRetrogradeTxt
        {
            get
            {
                return _natalUranusRetrogradeTxt;
            }
            set
            {
                _natalUranusRetrogradeTxt = value;
                OnPropertyChanged("natalUranusRetrogradeTxt");
            }
        }
        private double _natalUranusAngle;
        public double natalUranusAngle
        {
            get
            {
                return _natalUranusAngle;
            }
            set
            {
                _natalUranusAngle = value;
                OnPropertyChanged("natalUranusAngle");
            }
        }

        #endregion

        // 海王星
        #region
        private double _natalNeptuneX;
        public double natalNeptuneX
        {
            get
            {
                return _natalNeptuneX;
            }
            set
            {
                _natalNeptuneX = value;
                OnPropertyChanged("natalNeptunex");
            }
        }

        private double _natalNeptuneY;
        public double natalNeptuneY
        {
            get
            {
                return _natalNeptuneY;
            }
            set
            {
                _natalNeptuneY = value;
                OnPropertyChanged("natalNeptuney");
            }
        }
        private string _natalNeptuneTxt;
        public string natalNeptuneTxt
        {
            get
            {
                return _natalNeptuneTxt;
            }
            set
            {
                _natalNeptuneTxt = value;
                OnPropertyChanged("natalNeptuneTxt");
            }
        }
        private double _natalNeptuneDegreeX;
        public double natalNeptuneDegreeX
        {
            get
            {
                return _natalNeptuneDegreeX;
            }
            set
            {
                _natalNeptuneDegreeX = value;
                OnPropertyChanged("natalNeptuneDegreeX");
            }
        }

        private double _natalNeptuneDegreeY;
        public double natalNeptuneDegreeY
        {
            get
            {
                return _natalNeptuneDegreeY;
            }
            set
            {
                _natalNeptuneDegreeY = value;
                OnPropertyChanged("natalNeptuneDegreeY");
            }
        }
        private string _natalNeptuneDegreeTxt;
        public string natalNeptuneDegreeTxt
        {
            get
            {
                return _natalNeptuneDegreeTxt;
            }
            set
            {
                _natalNeptuneDegreeTxt = value;
                OnPropertyChanged("natalNeptuneDegreeTxt");
            }
        }
        private double _natalNeptuneSignX;
        public double natalNeptuneSignX
        {
            get
            {
                return _natalNeptuneSignX;
            }
            set
            {
                _natalNeptuneSignX = value;
                OnPropertyChanged("natalNeptuneSignX");
            }
        }

        private double _natalNeptuneSignY;
        public double natalNeptuneSignY
        {
            get
            {
                return _natalNeptuneSignY;
            }
            set
            {
                _natalNeptuneSignY = value;
                OnPropertyChanged("natalNeptuneSignY");
            }
        }
        private string _natalNeptuneSignTxt;
        public string natalNeptuneSignTxt
        {
            get
            {
                return _natalNeptuneSignTxt;
            }
            set
            {
                _natalNeptuneSignTxt = value;
                OnPropertyChanged("natalNeptuneSignTxt");
            }
        }
        private double _natalNeptuneMinuteX;
        public double natalNeptuneMinuteX
        {
            get
            {
                return _natalNeptuneMinuteX;
            }
            set
            {
                _natalNeptuneMinuteX = value;
                OnPropertyChanged("natalNeptuneMinuteX");
            }
        }

        private double _natalNeptuneMinuteY;
        public double natalNeptuneMinuteY
        {
            get
            {
                return _natalNeptuneMinuteY;
            }
            set
            {
                _natalNeptuneMinuteY = value;
                OnPropertyChanged("natalNeptuneMinuteY");
            }
        }
        private string _natalNeptuneMinuteTxt;
        public string natalNeptuneMinuteTxt
        {
            get
            {
                return _natalNeptuneMinuteTxt;
            }
            set
            {
                _natalNeptuneMinuteTxt = value;
                OnPropertyChanged("natalNeptuneMinuteTxt");
            }
        }
        private double _natalNeptuneRetrogradeX;
        public double natalNeptuneRetrogradeX
        {
            get
            {
                return _natalNeptuneRetrogradeX;
            }
            set
            {
                _natalNeptuneRetrogradeX = value;
                OnPropertyChanged("natalNeptuneRetrogradeX");
            }
        }

        private double _natalNeptuneRetrogradeY;
        public double natalNeptuneRetrogradeY
        {
            get
            {
                return _natalNeptuneRetrogradeY;
            }
            set
            {
                _natalNeptuneRetrogradeY = value;
                OnPropertyChanged("natalNeptuneRetrogradeY");
            }
        }
        private string _natalNeptuneRetrogradeTxt;
        public string natalNeptuneRetrogradeTxt
        {
            get
            {
                return _natalNeptuneRetrogradeTxt;
            }
            set
            {
                _natalNeptuneRetrogradeTxt = value;
                OnPropertyChanged("natalNeptuneRetrogradeTxt");
            }
        }
        private double _natalNeptuneAngle;
        public double natalNeptuneAngle
        {
            get
            {
                return _natalNeptuneAngle;
            }
            set
            {
                _natalNeptuneAngle = value;
                OnPropertyChanged("natalNeptuneAngle");
            }
        }

        #endregion

        // 冥王星
        #region
        private double _natalPlutoX;
        public double natalPlutoX
        {
            get
            {
                return _natalPlutoX;
            }
            set
            {
                _natalPlutoX = value;
                OnPropertyChanged("natalPlutox");
            }
        }

        private double _natalPlutoY;
        public double natalPlutoY
        {
            get
            {
                return _natalPlutoY;
            }
            set
            {
                _natalPlutoY = value;
                OnPropertyChanged("natalPlutoy");
            }
        }
        private string _natalPlutoTxt;
        public string natalPlutoTxt
        {
            get
            {
                return _natalPlutoTxt;
            }
            set
            {
                _natalPlutoTxt = value;
                OnPropertyChanged("natalPlutoTxt");
            }
        }
        private double _natalPlutoDegreeX;
        public double natalPlutoDegreeX
        {
            get
            {
                return _natalPlutoDegreeX;
            }
            set
            {
                _natalPlutoDegreeX = value;
                OnPropertyChanged("natalPlutoDegreeX");
            }
        }

        private double _natalPlutoDegreeY;
        public double natalPlutoDegreeY
        {
            get
            {
                return _natalPlutoDegreeY;
            }
            set
            {
                _natalPlutoDegreeY = value;
                OnPropertyChanged("natalPlutoDegreeY");
            }
        }
        private string _natalPlutoDegreeTxt;
        public string natalPlutoDegreeTxt
        {
            get
            {
                return _natalPlutoDegreeTxt;
            }
            set
            {
                _natalPlutoDegreeTxt = value;
                OnPropertyChanged("natalPlutoDegreeTxt");
            }
        }
        private double _natalPlutoSignX;
        public double natalPlutoSignX
        {
            get
            {
                return _natalPlutoSignX;
            }
            set
            {
                _natalPlutoSignX = value;
                OnPropertyChanged("natalPlutoSignX");
            }
        }

        private double _natalPlutoSignY;
        public double natalPlutoSignY
        {
            get
            {
                return _natalPlutoSignY;
            }
            set
            {
                _natalPlutoSignY = value;
                OnPropertyChanged("natalPlutoSignY");
            }
        }
        private string _natalPlutoSignTxt;
        public string natalPlutoSignTxt
        {
            get
            {
                return _natalPlutoSignTxt;
            }
            set
            {
                _natalPlutoSignTxt = value;
                OnPropertyChanged("natalPlutoSignTxt");
            }
        }
        private double _natalPlutoMinuteX;
        public double natalPlutoMinuteX
        {
            get
            {
                return _natalPlutoMinuteX;
            }
            set
            {
                _natalPlutoMinuteX = value;
                OnPropertyChanged("natalPlutoMinuteX");
            }
        }

        private double _natalPlutoMinuteY;
        public double natalPlutoMinuteY
        {
            get
            {
                return _natalPlutoMinuteY;
            }
            set
            {
                _natalPlutoMinuteY = value;
                OnPropertyChanged("natalPlutoMinuteY");
            }
        }
        private string _natalPlutoMinuteTxt;
        public string natalPlutoMinuteTxt
        {
            get
            {
                return _natalPlutoMinuteTxt;
            }
            set
            {
                _natalPlutoMinuteTxt = value;
                OnPropertyChanged("natalPlutoMinuteTxt");
            }
        }
        private double _natalPlutoRetrogradeX;
        public double natalPlutoRetrogradeX
        {
            get
            {
                return _natalPlutoRetrogradeX;
            }
            set
            {
                _natalPlutoRetrogradeX = value;
                OnPropertyChanged("natalPlutoRetrogradeX");
            }
        }

        private double _natalPlutoRetrogradeY;
        public double natalPlutoRetrogradeY
        {
            get
            {
                return _natalPlutoRetrogradeY;
            }
            set
            {
                _natalPlutoRetrogradeY = value;
                OnPropertyChanged("natalPlutoRetrogradeY");
            }
        }
        private string _natalPlutoRetrogradeTxt;
        public string natalPlutoRetrogradeTxt
        {
            get
            {
                return _natalPlutoRetrogradeTxt;
            }
            set
            {
                _natalPlutoRetrogradeTxt = value;
                OnPropertyChanged("natalPlutoRetrogradeTxt");
            }
        }
        private double _natalPlutoAngle;
        public double natalPlutoAngle
        {
            get
            {
                return _natalPlutoAngle;
            }
            set
            {
                _natalPlutoAngle = value;
                OnPropertyChanged("natalPlutoAngle");
            }
        }

        #endregion

        // 地球
        #region
        private double _natalEarthX;
        public double natalEarthX
        {
            get
            {
                return _natalEarthX;
            }
            set
            {
                _natalEarthX = value;
                OnPropertyChanged("natalEarthX");
            }
        }

        private double _natalEarthY;
        public double natalEarthY
        {
            get
            {
                return _natalEarthY;
            }
            set
            {
                _natalEarthY = value;
                OnPropertyChanged("natalEarthY");
            }
        }
        private string _natalEarthTxt;
        public string natalEarthTxt
        {
            get
            {
                return _natalEarthTxt;
            }
            set
            {
                _natalEarthTxt = value;
                OnPropertyChanged("natalEarthTxt");
            }
        }
        private double _natalEarthDegreeX;
        public double natalEarthDegreeX
        {
            get
            {
                return _natalEarthDegreeX;
            }
            set
            {
                _natalEarthDegreeX = value;
                OnPropertyChanged("natalEarthDegreeX");
            }
        }

        private double _natalEarthDegreeY;
        public double natalEarthDegreeY
        {
            get
            {
                return _natalEarthDegreeY;
            }
            set
            {
                _natalEarthDegreeY = value;
                OnPropertyChanged("natalEarthDegreeY");
            }
        }
        private string _natalEarthDegreeTxt;
        public string natalEarthDegreeTxt
        {
            get
            {
                return _natalEarthDegreeTxt;
            }
            set
            {
                _natalEarthDegreeTxt = value;
                OnPropertyChanged("natalEarthDegreeTxt");
            }
        }
        private double _natalEarthSignX;
        public double natalEarthSignX
        {
            get
            {
                return _natalEarthSignX;
            }
            set
            {
                _natalEarthSignX = value;
                OnPropertyChanged("natalEarthSignX");
            }
        }

        private double _natalEarthSignY;
        public double natalEarthSignY
        {
            get
            {
                return _natalEarthSignY;
            }
            set
            {
                _natalEarthSignY = value;
                OnPropertyChanged("natalEarthSignY");
            }
        }
        private string _natalEarthSignTxt;
        public string natalEarthSignTxt
        {
            get
            {
                return _natalEarthSignTxt;
            }
            set
            {
                _natalEarthSignTxt = value;
                OnPropertyChanged("natalEarthSignTxt");
            }
        }
        private double _natalEarthMinuteX;
        public double natalEarthMinuteX
        {
            get
            {
                return _natalEarthMinuteX;
            }
            set
            {
                _natalEarthMinuteX = value;
                OnPropertyChanged("natalEarthMinuteX");
            }
        }

        private double _natalEarthMinuteY;
        public double natalEarthMinuteY
        {
            get
            {
                return _natalEarthMinuteY;
            }
            set
            {
                _natalEarthMinuteY = value;
                OnPropertyChanged("natalEarthMinuteY");
            }
        }
        private string _natalEarthMinuteTxt;
        public string natalEarthMinuteTxt
        {
            get
            {
                return _natalEarthMinuteTxt;
            }
            set
            {
                _natalEarthMinuteTxt = value;
                OnPropertyChanged("natalEarthMinuteTxt");
            }
        }
        private double _natalEarthRetrogradeX;
        public double natalEarthRetrogradeX
        {
            get
            {
                return _natalEarthRetrogradeX;
            }
            set
            {
                _natalEarthRetrogradeX = value;
                OnPropertyChanged("natalEarthRetrogradeX");
            }
        }

        private double _natalEarthRetrogradeY;
        public double natalEarthRetrogradeY
        {
            get
            {
                return _natalEarthRetrogradeY;
            }
            set
            {
                _natalEarthRetrogradeY = value;
                OnPropertyChanged("natalEarthRetrogradeY");
            }
        }
        private string _natalEarthRetrogradeTxt;
        public string natalEarthRetrogradeTxt
        {
            get
            {
                return _natalEarthRetrogradeTxt;
            }
            set
            {
                _natalEarthRetrogradeTxt = value;
                OnPropertyChanged("natalEarthRetrogradeTxt");
            }
        }
        private double _natalEarthAngle;
        public double natalEarthAngle
        {
            get
            {
                return _natalEarthAngle;
            }
            set
            {
                _natalEarthAngle = value;
                OnPropertyChanged("natalEarthAngle");
            }
        }

        #endregion

        // ヘッド
        #region
        private double _natalDHX;
        public double natalDHX
        {
            get
            {
                return _natalDHX;
            }
            set
            {
                _natalDHX = value;
                OnPropertyChanged("natalDHX");
            }
        }

        private double _natalDHY;
        public double natalDHY
        {
            get
            {
                return _natalDHY;
            }
            set
            {
                _natalDHY = value;
                OnPropertyChanged("natalDHY");
            }
        }
        private string _natalDHTxt;
        public string natalDHTxt
        {
            get
            {
                return _natalDHTxt;
            }
            set
            {
                _natalDHTxt = value;
                OnPropertyChanged("natalDHTxt");
            }
        }
        private double _natalDHDegreeX;
        public double natalDHDegreeX
        {
            get
            {
                return _natalDHDegreeX;
            }
            set
            {
                _natalDHDegreeX = value;
                OnPropertyChanged("natalDHDegreeX");
            }
        }

        private double _natalDHDegreeY;
        public double natalDHDegreeY
        {
            get
            {
                return _natalDHDegreeY;
            }
            set
            {
                _natalDHDegreeY = value;
                OnPropertyChanged("natalDHDegreeY");
            }
        }
        private string _natalDHDegreeTxt;
        public string natalDHDegreeTxt
        {
            get
            {
                return _natalDHDegreeTxt;
            }
            set
            {
                _natalDHDegreeTxt = value;
                OnPropertyChanged("natalDHDegreeTxt");
            }
        }
        private double _natalDHSignX;
        public double natalDHSignX
        {
            get
            {
                return _natalDHSignX;
            }
            set
            {
                _natalDHSignX = value;
                OnPropertyChanged("natalDHSignX");
            }
        }

        private double _natalDHSignY;
        public double natalDHSignY
        {
            get
            {
                return _natalDHSignY;
            }
            set
            {
                _natalDHSignY = value;
                OnPropertyChanged("natalDHSignY");
            }
        }
        private string _natalDHSignTxt;
        public string natalDHSignTxt
        {
            get
            {
                return _natalDHSignTxt;
            }
            set
            {
                _natalDHSignTxt = value;
                OnPropertyChanged("natalDHSignTxt");
            }
        }
        private double _natalDHMinuteX;
        public double natalDHMinuteX
        {
            get
            {
                return _natalDHMinuteX;
            }
            set
            {
                _natalDHMinuteX = value;
                OnPropertyChanged("natalDHMinuteX");
            }
        }

        private double _natalDHMinuteY;
        public double natalDHMinuteY
        {
            get
            {
                return _natalDHMinuteY;
            }
            set
            {
                _natalDHMinuteY = value;
                OnPropertyChanged("natalDHMinuteY");
            }
        }
        private string _natalDHMinuteTxt;
        public string natalDHMinuteTxt
        {
            get
            {
                return _natalDHMinuteTxt;
            }
            set
            {
                _natalDHMinuteTxt = value;
                OnPropertyChanged("natalDHMinuteTxt");
            }
        }
        private double _natalDHRetrogradeX;
        public double natalDHRetrogradeX
        {
            get
            {
                return _natalDHRetrogradeX;
            }
            set
            {
                _natalDHRetrogradeX = value;
                OnPropertyChanged("natalDHRetrogradeX");
            }
        }

        private double _natalDHRetrogradeY;
        public double natalDHRetrogradeY
        {
            get
            {
                return _natalDHRetrogradeY;
            }
            set
            {
                _natalDHRetrogradeY = value;
                OnPropertyChanged("natalDHRetrogradeY");
            }
        }
        private string _natalDHRetrogradeTxt;
        public string natalDHRetrogradeTxt
        {
            get
            {
                return _natalDHRetrogradeTxt;
            }
            set
            {
                _natalDHRetrogradeTxt = value;
                OnPropertyChanged("natalDHRetrogradeTxt");
            }
        }
        private double _natalDHAngle;
        public double natalDHAngle
        {
            get
            {
                return _natalDHAngle;
            }
            set
            {
                _natalDHAngle = value;
                OnPropertyChanged("natalDHAngle");
            }
        }

        #endregion

        protected void OnPropertyChanged(string propertyname)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }

        }
    }
}
