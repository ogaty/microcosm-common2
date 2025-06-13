//
//  wrap_swe_utc_to_jd.c
//  swissEphemeris
//
//  Created by 緒形雄二 on 2024/02/15.
//

#include "wrap_swe_utc_to_jd.h"
#include "swephexp.h"

#include <stdio.h>
#include <dirent.h>


int wrap_swe_utc_to_jd(int32 utcYear, int32 utcMonth, int32 utcDay, int32 utcHour, int32 utcMinute, double utcSecond, int32 gregFlag, double* dret, char* serr) {
    swe_utc_to_jd(utcYear, utcMonth, utcDay, utcHour, utcMinute, utcSecond, gregFlag, dret, serr);
    return 0;
}
