//
//  wrap_swe_get_ayanamsa_ex_ut.c
//  swissEphemeris
//
//  Created by 緒形雄二 on 2024/07/18.
//

#include "wrap_swe_get_ayanamsa_ex_ut.h"
#include "swephexp.h"

int wrap_swe_get_ayanamsa_ex_ut(int tjd_ut, int iflag, double *daya, char *serr) {
    return swe_get_ayanamsa_ex_ut(tjd_ut, iflag, daya, serr);
}
