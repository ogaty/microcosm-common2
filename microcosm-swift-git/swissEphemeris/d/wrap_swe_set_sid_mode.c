//
//  wrap_swe_set_sid_mode.c
//  swissEphemeris
//
//  Created by 緒形雄二 on 2024/07/18.
//

#include "wrap_swe_set_sid_mode.h"
#include "swephexp.h"

void wrap_swe_set_sid_mode(int32 sid_mode) {
    swe_set_sid_mode(sid_mode, 0, 0);
}
