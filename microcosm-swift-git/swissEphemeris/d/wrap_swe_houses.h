//
//  wrap_swe_houses.h
//  swissEphemeris
//
//  Created by 緒形雄二 on 2024/02/15.
//

#ifndef wrap_swe_houses_h
#define wrap_swe_houses_h

#include <stdio.h>
int wrap_swe_houses(
        double tjd_ut, double geolat, double geolon, int hsys,
    double *cusps, double *ascmc);

#endif /* wrap_swe_houses_h */
