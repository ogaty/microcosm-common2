//
//  wrap_swe_calc_ut.c
//  swissEphemeris
//
//  Created by 緒形雄二 on 2024/02/15.
//

#include "wrap_swe_calc_ut.h"
#include <dirent.h>

int wrap_swe_calc_ut(double tjd_ut, int32 ipl, int32 iflag, double *xx, char *serr) {
    
    swe_calc_ut(tjd_ut, ipl, iflag, xx, serr);
    
    return 0;
}
