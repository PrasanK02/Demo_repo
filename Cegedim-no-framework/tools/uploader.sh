#!/bin/sh
CONSOLE_PATH='Cegedim/packages/Xamarin.UITest.0.6.6/tools/test-cloud.exe'
TEST_DIR_IOS='Cegedim/Cegedim.Test/bin/Debug'
# testclouddemo
ACCOUNT='e2756ca2130df973d555d7a5efe51d43'
IPA='Cegedim/cegedim.ipa'
DSYM='Cegedim/MI.app.DSYM'

# Pull the ipa
# This sharelink may expire
curl -L https://www.dropbox.com/s/j7ej40nmddvm7m9/MI.ipa\?dl\=0 -o $IPA
curl -L https://www.dropbox.com/sh/n5nt4ff70b075pv/AADSedd-Ezrq3Cfnd9CJ410fa\?dl\=0 -o $DSYM
 
# No iOS 8
SMALL_SUBSET='0c5697a3'
 
# No iOS 8
LARGE_SUBSET='12aef708'
 
echo "Uploading the file $IPA to Test Cloud on device set $SMALL_SUBSET"
mono $CONSOLE_PATH submit "$IPA" $ACCOUNT --devices $SMALL_SUBSET_SUBSET --assembly-dir $TEST_DIR_IOS --dsym $DSYM --series "Jenkins" --locale "en_US" --nunit-xml report.xml
