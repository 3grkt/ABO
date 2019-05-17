var aboUtils = {
    setGridCheckBox: function (headerId, chkIdPrefix, submitValidation) {
        // Trigger event for header checkbox
        $('#' + headerId).change(function () {
            var self = $(this);
            $('[id^=' + chkIdPrefix + ']').each(function (index, element) {
                $(element).prop('checked', self.prop('checked'));
            });
        });

        // Trigger event for item checkbox
        $('[id^=' + chkIdPrefix + ']').each(function (index, element) {
            $(element).change(function () {
                var checkboxes = $('[id^=' + chkIdPrefix + ']');
                var total = checkboxes.length;
                var checkedCount = 0;
                for (var i = 0; i < total; i++) {
                    if ($(checkboxes[i]).prop('checked'))
                        checkedCount++;
                }
                $('#' + headerId).prop('checked', total == checkedCount);
            });
        });

        // Validate submission
        if (submitValidation != null) {
            $.each(submitValidation.buttons, function (index, value) {
                value.click(function () {
                    if ($('[id^=' + chkIdPrefix + ']:checked').length == 0){
                        alert(submitValidation.message);
                        return false;
                    }
                });
            });
        }
    },
    hideErrorMessages: function() {
        $('.alert.alert-danger').remove();
    },
    // Create a proxy to intercept the passed-in func before and after the function is called.
    actionTrigger: function(fn, before, after) {
        return function () {
            before && before.apply(this, arguments);
            
            fn && fn.apply(this, arguments);

            after && after.apply(this, arguments);
        }
    }
}