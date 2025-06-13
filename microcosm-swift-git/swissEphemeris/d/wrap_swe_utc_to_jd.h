//
//  wrap_swe_utc_to_jd.h
//  swissEphemeris
//
//  Created by 緒形雄二 on 2024/02/15.
//

#ifndef wrap_swe_utc_to_jd_h
#define wrap_swe_utc_to_jd_h

#include <stdio.h>

int wrap_swe_utc_to_jd(int32 utcYear, int32 utcMonth, int32 utcDay, int32 utcHour, int32 utcMinute, double utcSecond, int32 gregFlag, double* dret, char* serr);

#endif /* wrap_swe_utc_to_jd_h */
