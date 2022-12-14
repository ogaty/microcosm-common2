using System;
using microcosmMac2.Calc;
using microcosmMac2.Common;

namespace microcosmMac2
{
	partial class ViewController
	{
        public string GetTimeSet()
        {
            return timesetterChangeButton.SelectedItem.Title;
        }

        /// <summary>
        /// ">"ボタン
        /// </summary>
        /// <param name="sender"></param>
        partial void TimeSetRightClicked(Foundation.NSObject sender)
        {
            string currentTime = GetTimeSet();
            DateTime now = udata1.GetDateTime();
            if (appDelegate.currentSpanType == SpanType.UNIT)
            {
                if (currentTime == "User1")
                {
                    now = udata1.GetDateTime().AddSeconds(plusUnit);
                    udata1.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    now = udata2.GetDateTime().AddSeconds(plusUnit);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    now = edata1.GetDateTime().AddSeconds(plusUnit);
                    edata1.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    now = edata2.GetDateTime().AddSeconds(plusUnit);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.NEWMOON)
            {
                if (currentTime == "User1")
                {
                    MoonCalc moon = new MoonCalc(configData);
                    now = moon.GetNewMoon(udata1.GetDateTime(), udata1.timezone);
                    udata1.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    now = udata2.GetDateTime().AddSeconds(plusUnit);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    now = edata1.GetDateTime().AddSeconds(plusUnit);
                    edata1.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    now = edata2.GetDateTime().AddSeconds(plusUnit);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.FULLMOON)
            {
                if (currentTime == "User1")
                {
                    now = udata1.GetDateTime().AddSeconds(plusUnit);
                    udata1.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    now = udata2.GetDateTime().AddSeconds(plusUnit);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    now = edata1.GetDateTime().AddSeconds(plusUnit);
                    edata1.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    now = edata2.GetDateTime().AddSeconds(plusUnit);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.SOLARRETURN)
            {
                EclipseCalc eclipse = calc.GetEclipseInstance();
                if (currentTime == "User1")
                {
                    DateTime target = eclipse.GetEclipse(udata1.GetDateTime(), udata1.timezone, 0, list1[CommonData.ZODIAC_SUN].absolute_position, true);
                    udata1.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    DateTime target = eclipse.GetEclipse(udata2.GetDateTime(), udata2.timezone, 0, list1[CommonData.ZODIAC_SUN].absolute_position, true);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    DateTime target = eclipse.GetEclipse(edata1.GetDateTime(), edata1.timezone, 0, list1[CommonData.ZODIAC_SUN].absolute_position, true);
                    edata1.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    DateTime target = eclipse.GetEclipse(edata2.GetDateTime(), edata2.timezone, 0, list1[CommonData.ZODIAC_SUN].absolute_position, true);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.SOLARINGRESS)
            {
                EclipseCalc eclipse = calc.GetEclipseInstance();
                double targetDegree = GetNextIngressDegree(list1[CommonData.ZODIAC_SUN].absolute_position);
                if (currentTime == "User1")
                {
                    DateTime target = eclipse.GetEclipse(udata1.GetDateTime(), udata1.timezone, 0, targetDegree, true);
                    udata1.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    DateTime target = eclipse.GetEclipse(udata2.GetDateTime(), udata2.timezone, 0, targetDegree, true);
                    udata2.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    DateTime target = eclipse.GetEclipse(edata1.GetDateTime(), edata1.timezone, 0, targetDegree, true);
                    edata1.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    DateTime target = eclipse.GetEclipse(edata2.GetDateTime(), edata2.timezone, 0, targetDegree, true);
                    edata2.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.MOONINGRESS)
            {
                EclipseCalc eclipse = calc.GetEclipseInstance();
                double targetDegree = GetNextIngressDegree(list1[CommonData.ZODIAC_SUN].absolute_position);
                if (currentTime == "User1")
                {
                    DateTime target = eclipse.GetEclipse(udata1.GetDateTime(), udata1.timezone, 1, targetDegree, true);
                    udata1.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    DateTime target = eclipse.GetEclipse(udata2.GetDateTime(), udata2.timezone, 1, targetDegree, true);
                    udata2.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    DateTime target = eclipse.GetEclipse(edata1.GetDateTime(), edata1.timezone, 1, targetDegree, true);
                    edata1.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    DateTime target = eclipse.GetEclipse(edata2.GetDateTime(), edata2.timezone, 1, targetDegree, true);
                    edata2.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            ReRender();
        }

        /// <summary>
        /// "<"ボタン
        /// </summary>
        /// <param name="sender"></param>
        partial void TimeSetLeftClicked(Foundation.NSObject sender)
        {
            string currentTime = GetTimeSet();
            DateTime now = udata1.GetDateTime();
            if (appDelegate.currentSpanType == SpanType.UNIT)
            {
                if (currentTime == "User1")
                {
                    now = udata1.GetDateTime().AddSeconds(-1 * plusUnit);
                    udata1.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    now = udata2.GetDateTime().AddSeconds(-1 * plusUnit);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    now = edata1.GetDateTime().AddSeconds(-1 * plusUnit);
                    edata1.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    now = edata2.GetDateTime().AddSeconds(-1 * plusUnit);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.NEWMOON)
            {
                if (currentTime == "User1")
                {
                    now = udata1.GetDateTime().AddSeconds(plusUnit);
                    udata1.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    now = udata2.GetDateTime().AddSeconds(plusUnit);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    now = edata1.GetDateTime().AddSeconds(plusUnit);
                    edata1.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    now = edata2.GetDateTime().AddSeconds(plusUnit);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.FULLMOON)
            {
                if (currentTime == "User1")
                {
                    now = udata1.GetDateTime().AddSeconds(plusUnit);
                    udata1.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    now = udata2.GetDateTime().AddSeconds(plusUnit);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    now = edata1.GetDateTime().AddSeconds(plusUnit);
                    edata1.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    now = edata2.GetDateTime().AddSeconds(plusUnit);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.SOLARRETURN)
            {
                EclipseCalc eclipse = calc.GetEclipseInstance();
                if (currentTime == "User1")
                {
                    DateTime target = eclipse.GetEclipse(udata1.GetDateTime(), udata1.timezone, 0, list1[CommonData.ZODIAC_SUN].absolute_position, false);
                    udata1.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    DateTime target = eclipse.GetEclipse(udata2.GetDateTime(), udata2.timezone, 0, list1[CommonData.ZODIAC_SUN].absolute_position, false);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    DateTime target = eclipse.GetEclipse(edata1.GetDateTime(), edata1.timezone, 0, list1[CommonData.ZODIAC_SUN].absolute_position, false);
                    edata1.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    DateTime target = eclipse.GetEclipse(edata2.GetDateTime(), edata2.timezone, 0, list1[CommonData.ZODIAC_SUN].absolute_position, false);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.SOLARINGRESS)
            {
                EclipseCalc eclipse = calc.GetEclipseInstance();
                double targetDegree = GetPrevIngressDegree(list1[CommonData.ZODIAC_SUN].absolute_position);
                if (currentTime == "User1")
                {
                    DateTime target = eclipse.GetEclipse(udata1.GetDateTime(), udata1.timezone, 0, targetDegree, false);
                    udata1.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    DateTime target = eclipse.GetEclipse(udata2.GetDateTime(), udata2.timezone, 0, targetDegree, false);
                    udata2.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    DateTime target = eclipse.GetEclipse(edata1.GetDateTime(), edata1.timezone, 0, targetDegree, false);
                    edata1.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    DateTime target = eclipse.GetEclipse(edata2.GetDateTime(), edata2.timezone, 0, targetDegree, false);
                    edata2.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            else if (appDelegate.currentSpanType == SpanType.MOONINGRESS)
            {
                EclipseCalc eclipse = calc.GetEclipseInstance();
                double targetDegree = GetPrevIngressDegree(list1[CommonData.ZODIAC_SUN].absolute_position);
                if (currentTime == "User1")
                {
                    DateTime target = eclipse.GetEclipse(udata1.GetDateTime(), udata1.timezone, 1, targetDegree, false);
                    udata1.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    DateTime target = eclipse.GetEclipse(udata2.GetDateTime(), udata2.timezone, 1, targetDegree, false);
                    udata2.SetDateTime(target);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    DateTime target = eclipse.GetEclipse(edata1.GetDateTime(), edata1.timezone, 1, targetDegree, false);
                    edata1.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    DateTime target = eclipse.GetEclipse(edata2.GetDateTime(), edata2.timezone, 1, targetDegree, false);
                    edata2.SetDateTime(target);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            ReRender();
        }

        partial void TimeSetNowClicked(Foundation.NSObject sender)
        {
            string currentTime = GetTimeSet();
            if (currentTime == "User1")
            {
                udata1.SetDateTime(DateTime.Now);
                ReCalc();
                RefreshUserBox(0, udata1);
            }
            else if (currentTime == "User2")
            {
                udata2.SetDateTime(DateTime.Now);
                ReCalc();
                RefreshUserBox(1, udata1);
            }
            else if (currentTime == "Event1")
            {
                edata1.SetDateTime(DateTime.Now);
                ReCalc();
                RefreshEventBox(0, edata1);
            }
            else if (currentTime == "Event2")
            {
                edata2.SetDateTime(DateTime.Now);
                ReCalc();
                RefreshEventBox(1, edata2);
            }
            ReRender();
        }

        /// <summary>
        /// 1重なら表示されている円、3重なら外側をNowにする
        /// </summary>
        public void TimeSetNowCurrentBand()
        {
            ETargetUser currentTime = ETargetUser.USER1;

            if (appDelegate.bands == 1)
            {
                currentTime = calcTargetUser[0];
            }
            else if (appDelegate.bands == 2)
            {
                currentTime = calcTargetUser[1];
            }
            else if (appDelegate.bands == 3)
            {
                currentTime = calcTargetUser[2];
            }
            DateTime now = DateTime.Now;
            if (currentTime == ETargetUser.USER1)
            {
                udata1.SetDateTime(now);
                ReCalc();
                RefreshUserBox(0, udata1);
            }
            else if (currentTime == ETargetUser.USER2)
            {
                udata2.SetDateTime(now);
                ReCalc();
                RefreshUserBox(1, udata2);
            }
            else if (currentTime == ETargetUser.EVENT1)
            {
                edata1.SetDateTime(now);
                ReCalc();
                RefreshEventBox(0, edata1);
            }
            else if (currentTime == ETargetUser.EVENT2)
            {
                edata2.SetDateTime(now);
                ReCalc();
                RefreshEventBox(1, edata2);
            }
            ReRender();

        }

        /// <summary>
        /// 1重なら表示されている円、3重なら外側かなぁ
        /// </summary>
        /// <param name="seconds"></param>
        public void TimeSetAny(int seconds)
        {
            ETargetUser currentTime = ETargetUser.USER1;

            if (appDelegate.bands == 1)
            {
                currentTime = calcTargetUser[0];
            }
            else if (appDelegate.bands == 2)
            {
                currentTime = calcTargetUser[1];
            }
            else if (appDelegate.bands == 3)
            {
                currentTime = calcTargetUser[2];
            }
            DateTime now = udata1.GetDateTime();
            if (currentTime == ETargetUser.USER1)
            {
                now = udata1.GetDateTime().AddSeconds(seconds);
                udata1.SetDateTime(now);
                ReCalc();
                RefreshUserBox(0, udata1);
            }
            else if (currentTime == ETargetUser.USER2)
            {
                now = udata2.GetDateTime().AddSeconds(seconds);
                udata2.SetDateTime(now);
                ReCalc();
                RefreshUserBox(1, udata2);
            }
            else if (currentTime == ETargetUser.EVENT1)
            {
                now = edata1.GetDateTime().AddSeconds(seconds);
                edata1.SetDateTime(now);
                ReCalc();
                RefreshEventBox(0, edata1);
            }
            else if (currentTime == ETargetUser.EVENT2)
            {
                now = edata2.GetDateTime().AddSeconds(seconds);
                edata2.SetDateTime(now);
                ReCalc();
                RefreshEventBox(1, edata2);
            }
            ReRender();

        }


    }
}

