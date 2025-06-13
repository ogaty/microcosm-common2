//
//  wrap_swe_houses.c
//  swissEphemeris
//
//  Created by 緒形雄二 on 2024/02/15.
//

#include "wrap_swe_houses.h"

/* housasp.c
 * cusps are returned in double cusp[13],
 *                           or cusp[37] with house system 'G'.
 * cusp[1...12] houses 1 - 12
 * additional points are returned in ascmc[10].
 * ascmc[0] = ascendant
 * ascmc[1] = mc
 * ascmc[2] = armc
 * ascmc[3] = vertex
 * ascmc[4] = equasc            * "equatorial ascendant" *
 * ascmc[5] = coasc1            * "co-ascendant" (W. Koch) *
 * ascmc[6] = coasc2            * "co-ascendant" (M. Munkasey) *
 * ascmc[7] = polasc            * "polar ascendant" (M. Munkasey) *
 */
int wrap_swe_houses(
                    double tjd_ut, double geolat, double geolon, int hsys,
                    double *cusps, double *ascmc) {
    swe_houses(tjd_ut, geolat, geolon, hsys, cusps, ascmc);
    return 0;
}
