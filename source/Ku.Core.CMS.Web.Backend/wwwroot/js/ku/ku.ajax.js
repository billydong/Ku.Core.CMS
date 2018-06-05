﻿if (!window.ku) {
    window.ku = {};
}

/// <summary>ajax 封装, 基于jquery.ajax</summary>
(function () {
    function _getAuthHeader() {
        return {};
    }
    function _getUrl(url, data) {
        return url;
    }
    function _getJsonPUrl(url) {
        url = _getUrl(url);
        var index;
        //检测url中是否已包含了callback参数
        if ((index = url.indexOf("?")) > 0) {
            if (url.indexOf("callback", index) > 0) {
                return url;
            }
            url += "&";
        } else {
            url += "?";
        }
        url += "callback=?&format=jsonp";
        return url;
    }
    function _preCheckResponse(r, ajaxParam) {
        //if (r && r.code && (r.code === 401 || r.code === 402)) {
        //    ec.common.msg.showTip("您还未登陆,转入登录页", 1500, function () {
        //        window.location.href = "/login.aspx?returnurl=" + encodeURI(window.location.href);
        //    });
        //    return false;
        //}
        //if (r && r.code && (r.code === 2001 || r.code === 2002)) {
        //    //需要验证码
        //    ec.common.showVCode(ajaxParam);
        //    return false;
        //}
        return true;
    }
    function _getLoading() {
        var $loading = $("#loading");
        if (!$loading.length) {
            $loading = $("<div class='loading' id='loading'><div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div></div>");
            $(document.body).append($loading);
        }
        return $loading;
    }
    ku.ajax = {
        get: function (url, data, successCallBack, errorCallBack) {
            var ajaxParam = {
                method: "get",
                url: url,
                data: data,
                successCallBack: successCallBack,
                errorCallBack: errorCallBack
            };
            $.ajax({
                url: _getUrl(url),
                method: "get",
                dataType: "json",
                headers: _getAuthHeader(),
                data: data,
                beforeSend: function () {
                    ku.page.msg.showLoad();
                },
                success: function (r) {
                    if (_preCheckResponse(r, ajaxParam)) {
                        successCallBack(r);
                    }
                },
                complete: function () {
                    ku.page.msg.hideLoad();
                },
                error: function (xhr, status, errorThrown) {
                    if (xhr.status == 403) {
                        ku.page.msg.alert(`无权操作！`, null, { icon: 5 });
                    } else {
                        if (errorCallBack) {
                            errorCallBack(xhr, status, errorThrown);
                        } else {
                            ku.page.msg.alert(`调用出错：{${xhr.status}}${status}`, null, { icon: 5 });
                        }
                    }
                }
            });
        },
        syncget: function (url, data, successCallBack, errorCallBack) {
            $.ajax({
                url: _getUrl(url),
                method: "get",
                dataType: "json",
                headers: _getAuthHeader(),
                data: data,
                async: false,
                beforeSend: function () {
                    _getLoading().show();
                },
                success: function (r) {
                    _preCheckResponse(r);
                    successCallBack(r);
                },
                complete: function () {
                    _getLoading().hide();
                },
                error: function (xhr, status, errorThrown) {
                    if (xhr.status == 403) {
                        ku.page.msg.alert(`无权操作！`, null, { icon: 5 });
                    } else {
                        if (errorCallBack) {
                            errorCallBack(xhr, status, errorThrown);
                        } else {
                            ku.page.msg.alert(`调用出错：{${xhr.status}}${status}`, null, { icon: 5 });
                        }
                    }
                }
            });
        },
        post: function (url, data, successCallBack, errorCallBack) {
            var ajaxParam = {
                method: "post",
                url: url,
                data: data,
                successCallBack: successCallBack,
                errorCallBack: errorCallBack
            };
            $.ajax({
                url: _getUrl(url),
                method: "post",
                contentType: "application/x-www-form-urlencoded",
                dataType: "json",
                headers: _getAuthHeader(),
                data: data,
                beforeSend: function () {
                    _getLoading().show();
                },
                success: function (r) {
                    if (_preCheckResponse(r, ajaxParam)) {
                        successCallBack(r);
                    }
                },
                complete: function () {
                    _getLoading().hide();
                },
                error: function (xhr, status, errorThrown) {
                    if (xhr.status == 403) {
                        ku.page.msg.alert(`无权操作！`, null, { icon: 5 });
                    } else {
                        if (errorCallBack) {
                            errorCallBack(xhr, status, errorThrown);
                        } else {
                            ku.page.msg.alert(`调用出错：{${xhr.status}}${status}`, null, { icon: 5 });
                        }
                    }
                }
            });
        },
        delete: function (url, data, successCallBack, errorCallBack) {
            var ajaxParam = {
                method: "delete",
                url: url,
                data: data,
                successCallBack: successCallBack,
                errorCallBack: errorCallBack
            };
            $.ajax({
                url: _getUrl(url),
                method: "delete",
                contentType: "application/x-www-form-urlencoded",
                dataType: "json",
                headers: _getAuthHeader(),
                data: data,
                beforeSend: function () {
                    _getLoading().show();
                },
                success: function (r) {
                    if (_preCheckResponse(r, ajaxParam)) {
                        successCallBack(r);
                    }
                },
                complete: function () {
                    _getLoading().hide();
                },
                error: function (xhr, status, errorThrown) {
                    if (xhr.status == 403) {
                        ku.page.msg.alert(`无权操作！`, null, { icon: 5 });
                    } else {
                        if (errorCallBack) {
                            errorCallBack(xhr, status, errorThrown);
                        } else {
                            ku.page.msg.alert(`调用出错：{${xhr.status}}${status}`, null, { icon: 5 });
                        }
                    }
                }
            });
        },
        jsonp: function (url, data, successCallBack, errorCallBack) {
            $.ajax({
                url: _getJsonPUrl(url),
                dataType: "jsonp",
                data: data,
                //headers: _getAuthHeader(),
                success: function (r) {
                    successCallBack(r);
                },
                error: function (xhr, status, errorThrown) {
                    if (errorCallBack) {
                        errorCallBack(xhr, status, errorThrown);
                    }
                }
            });
        }
    }
})();
