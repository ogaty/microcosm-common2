//
//  wrap_calc_timezone.c
//  swissEphemeris
//
//  Created by 緒形雄二 on 2023/09/10.
//

#include "wrap_utc_timezone.h"
#include "swephexp.h"
#include <dirent.h>

int wrap_utc_timezone(int32* utcYear, int32* utcMonth, int32* utcDay, int32* utcHour, int32* utcMinute, double* utcSecond, double timeZone) {
    int Year = *utcYear;
    int Month = *utcMonth;
    int Day = *utcDay;
    int Hour = *utcHour;
    int Minute = *utcMinute;
    int Second = *utcSecond;
    double timezone = timeZone;
    //int utcYear;
    //int utcMonth;
    //int utcDay;
    //int utcHour;
    //int utcMinute;
    //double utcSecond;
//    int gregFlag = 1;
//    double dret[2];
//    char serr[256];
//    double x[6];
    
//    int planetId = 0;
//    int flag = 0;
//    flag |= SEFLG_SWIEPH | SEFLG_SPEED;

    
    printf("%f\n", utcHour);
    swe_utc_time_zone(Year, Month, Day, Hour, Minute, Second, timezone,
                      utcYear, utcMonth, utcDay, utcHour, utcMinute, utcSecond);
    printf("%f\n", utcHour);
    //swe_utc_to_jd(*utcYear, utcMonth, utcDay, *utcHour, utcMinute, utcSecond, gregFlag, dret, serr);

    //swe_calc_ut(dret[1], planetId, flag, x, serr);

    //printf("%f\n", x[0]);

    return 0;
}
