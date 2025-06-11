//
//  wrap_swe_calc_ut.h
//  swissEphemeris
//
//  Created by 緒形雄二 on 2024/02/15.
//

#ifndef wrap_swe_calc_ut_h
#define wrap_swe_calc_ut_h

#include <stdio.h>

int wrap_swe_calc_ut(double tjd_ut, int32 ipl, int32 iflag,
    double *xx, char *serr);

#endif /* wrap_swe_calc_ut_h */
