#include "wrap_set_ephe_path.h"
#include <dirent.h>

int wrap_set_ephe_path(char* ephepath) {
    // char ephepath[AS_MAXCH];
    //sprintf(ephepath, "./");
    
    swe_set_ephe_path(ephepath);
    
    struct dirent *dirst;
    
    DIR *dp = opendir(ephepath);
//        while((dirst = readdir(dp)) != NULL)
//        {
//            printf("レコード長 : [%d]\n", dirst->d_reclen);
//            printf("ディレクトリ名の文字列の長さ : [%d]\n", dirst->d_namlen);
//            printf("ディレクトリ名 : [%s]\n\n", dirst->d_name);
//        }
//        closedir(dp);
    
    
    int Year = 2023;
    int Month = 9;
    int Day = 1;
    int Hour = 12;
    int Minute = 0;
    int Second = 0;
    double timezone = 9.0;
    int utcYear;
    int utcMonth;
    int utcDay;
    int utcHour;
    int utcMinute;
    double utcSecond;
    int gregFlag = 1;
    double dret[2];
    char serr[256];
    double x[6];
    
    int planetId = 0;
    int flag = 0;
    flag |= SEFLG_SWIEPH | SEFLG_SPEED;

    
    swe_utc_time_zone(Year, Month, Day, Hour, Minute, Second, timezone,
                      &utcYear, &utcMonth, &utcDay, &utcHour, &utcMinute, &utcSecond);
    swe_utc_to_jd(utcYear, utcMonth, utcDay, utcHour, utcMinute, utcSecond, gregFlag, dret, serr);

    swe_calc_ut(dret[1], planetId, flag, x, serr);

    printf("%f\n", x[0]);

    return 0;
}
