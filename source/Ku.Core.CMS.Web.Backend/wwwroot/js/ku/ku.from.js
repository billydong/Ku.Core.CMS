﻿if (!window.ku) {
    window.ku = {};
}

(function ($) {
    function _handleMessage(reply, options) {
        ku.page.msg.hideLoad();
        if (!reply)
        { return false; }
        var success = reply.code === 0;
        if (success) {
            if (options.onSuccess && typeof options.onSuccess === "function") {
                return options.onSuccess(reply, options);
            }
            return true;
        } else {
            if (options.onError && typeof options.onError === "function") {
                var ret = options.onError(reply, options);
                if (ret) {
                    return false;
                }
            }
            ku.page.msg.tip(reply.message);
            return false;
        }
    }

    function _bind(target) {
        jQuery.getScript("/lib/jquery-form/jquery.form.js").done(function () {
            var $target = $(target);
            var opts = $target.data("options");
            var options = {
                type: opts.type,
                dataType: opts.dataType,
                beforeSubmit: function (arr, $from, options) {
                    var isValid = false;
                    if (opts.onBefore && typeof opts.onBefore === "function") {
                        isValid = opts.onBefore(arr, $from, opts);
                        if (!isValid) {
                            return false;
                        }
                    }
                    ku.page.msg.showLoad();
                    return true;
                },
                error: function (data) {
                    ku.page.msg.hideLoad();
                    ku.page.msg.alert(`有错误发生：(${data.status})${data.statusText}`, null, { icon: 5 });
                },
                success: function (reply) {
                    _handleMessage(reply, opts);
                }
            };
            $target.ajaxForm(options);
        }).fail(function () {
            ku.page.msg.alert(`脚本加载出错,请刷新页面重试！`, null, { icon: 5 });
        });
    }
    
    $.fn.kuForm = function (options, param) {
        if (typeof options === 'string') {
            return $.fn.kuForm.methods[options](this, param);
        }
        options = options || {};
        return this.each(function () {
            var opts = $.data(this, 'options');
            if (opts) {
                opts = $.extend(opts, options);
            } else {
                opts = $(this).data("opts") || {};
                opts = $.extend(opts, $.fn.kuForm.defaults, options);
                $.data(this, 'options', opts);
            }
            _bind(this);
        });
    };

    $.fn.kuForm.methods = {
        submit: function (target, options) {
            target.each(function () {
                if (options) {
                    var opts = $.data(this, 'options');
                    opts = $.extend(opts, options);
                }
                $(this).submit();
            });
        }
    };

    $.fn.kuForm.defaults = {
        type: 'POST',
        dataType: 'json'
    };

})(jQuery);