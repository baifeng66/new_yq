//var ApiBaseUrl = "http://99int.com:1912";
var ApiBaseUrl = "http://localhost:5179/";
var imt_token = sessionStorage.getItem("imt_token");
var imt_uid = sessionStorage.getItem("imt_uid");
var imt_last = sessionStorage.getItem("imt_last");
var imt_pwd = sessionStorage.getItem("imt_pwd");
jQuery.support.cors = true;
$.extend({
    POST: function (url, data, success, async) {
        if (async == undefined) {
            async = true;
        }
        $.ajax({
            url: ApiBaseUrl + url,
            type: "post",
            data: JSON.stringify(data),
            async: async,
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", "Bearer " + imt_token);
                request.setRequestHeader("uid", imt_uid);
                request.setRequestHeader("type", 0);
            },
            success: function (data) {
                if (data.code == 100) {
                    top.layer.alert('登录已过期，请重新登录',
                        { closeBtn: 0 },
                        function () {
                            top.layer.closeAll();
                            top.loginout();
                        });
                } else {
                    success(data);
                }
            },
            error: function (ms, s, e) {
                var res = {};
                res.code = 2;
                res.msg = s;
                res.data = '';
                res.count = 0;
                success(res);
            }
        });
    },
    POSTForm: function (url, data, success, async) {
        if (async == undefined) {
            async = true;
        }
        $.ajax({
            url: ApiBaseUrl + url,
            type: "post",
            data: data,
            async: async,
            //dataType: 'json',
            contentType: "application/x-www-form-urlencoded",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", "Bearer " + imt_token);
                request.setRequestHeader("uid", imt_uid);
                request.setRequestHeader("type", 0);
            },
            success: function (data) {
                if (data.code == 100) {
                    top.layer.alert('登录已过期，请重新登录',
                        { closeBtn: 0 },
                        function () {
                            top.layer.closeAll();
                            top.loginout();
                        });
                } else {
                    success(data);
                }
            },
            error: function (ms, s, e) {
                var res = {};
                res.code = 2;
                res.msg = s;
                res.data = '';
                res.count = 0;
                success(res);
            }
        });
    },
    GET: function (url, data, success, async) {
        if (async == undefined) {
            async = true;
        }
        $.ajax({
            url: ApiBaseUrl + url,
            type: "GET",
            data: data,
            dataType: 'json',
            async: async,
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", "Bearer " + imt_token);
                request.setRequestHeader("type", 0);
                request.setRequestHeader("uid", imt_uid);
            },
            success: function (data) {
                if (data.code == 100) {
                    top.layer.alert('登录已过期，请重新登录',
                        { closeBtn: 0 },
                        function () {
                            top.layer.closeAll();
                            top.loginout();
                        });
                }
                success(data);
            },
            error: function (ms, s, e) {
                var res = {};
                res.code = 1;
                res.msg = s;
                res.data = '';
                success(res);
            }
        });
    },
    DELETE: function (url, data, success) {

        $.ajax({
            url: ApiBaseUrl + url,
            type: "DELETE",
            data: data,
            async: false,
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", "Bearer " + imt_token);
                request.setRequestHeader("type", 0);
                request.setRequestHeader("uid", imt_uid);
            },
            success: function (data) {
                if (data.Code == 100) {
                    top.layer.alert('登录已过期，请重新登录',
                        { closeBtn: 0 },
                        function () {
                            top.layer.closeAll();
                            top.loginout();
                        });
                }
                success(data);
            },
            error: function (ms, s, e) {
                var res = {};
                res.code = 1;
                res.msg = s;
                res.data = '';
                success(res);
            }
        });
    },
    DeleteConfirm: function (url, data, success) {
        top.$.confirm('确定删除该数据吗？',
            function () {
                $.DELETE(url,
                    data,
                    function (data) {
                        if (data.Code == 0) {
                            top.layer('删除成功');
                            success(data);
                        } else {
                            top.layer(data.Msg);
                        }
                    },
                    false);
            });
    },
    //获取url参数
    getQueryVariable: function (variable) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) { return pair[1]; }
        }
        return (undefined);
    },
    //自动填写表单数据
    setFormData: function (jsonObj) {
        //var obj = eval("(" + jsonStr + ")");
        var obj = jsonObj;
        var key, value, tagName, type, arr;
        for (x in obj) {
            key = x;
            value = obj[x];

            $("[name='" + key + "'],[name='" + key + "[]']").each(function () {
                tagName = $(this)[0].tagName;
                type = $(this).attr('type');
                if (tagName == 'INPUT') {
                    if (type == 'radio') {
                        //$(this).attr('checked', $(this).val() == value);
                        if ($(this).val() == value) {
                            $(this).attr('checked', true);
                        }
                    } else if (type == 'checkbox') {
                        if (typeof (value) == 'string' && value.indexOf(",") != -1) {
                            arr = value.split(',');
                            for (var i = 0; i < arr.length; i++) {
                                if ($(this).val() == arr[i]) {
                                    $(this).attr('checked', true);
                                    break;
                                }
                            }
                        } else if (value instanceof Array) {
                            arr = value;
                            for (var i = 0; i < arr.length; i++) {
                                if ($(this).val() == arr[i]) {
                                    $(this).attr('checked', true);
                                    break;
                                }
                            }
                        } else {
                            if ($(this).val() == value) {
                                $(this).attr('checked', true);
                            }
                        }
                    } else {
                        if (value != "-1") {
                            $(this).val(value);
                        }
                    }
                } else if (tagName == 'TEXTAREA') {
                    $(this).val(value);
                } else if (tagName == 'DIV' || tagName == 'SPAN' || tagName == 'TH' || tagName == 'TD') {
                    if (value != null) {
                        $(this).text(value);
                    }
                } else if (tagName == 'SELECT') {
                    $(this).find("option[value='" + value + "']").attr("selected", true);
                }

            });
        }
    },
    //下载
    download: function (url) {
        // 会打开一个空白页下载，然后空白页消失，用户体验不好
        window.open(ApiBaseUrl + url + "&token=" + imt_token);

        //var $form = $('<form method="GET"></form>');
        //$form.attr('action', url);
        //$form.appendTo($('body'));
        //$form.submit();

        //try {
        //    var exportForm = $("<form action='" + ApiBaseUrl + url + "&token=" + imt_token + "' method='GET' ></form>")
        //    $(document.body).append(exportForm);
        //    exportForm.submit();
        //} catch (e) {
        //    console.log(e);
        //} finally {
        //    exportForm.remove();
        //}
    },
    //登录验证
    veriflogin: function (data) {
        if (data.code == 100) {
            top.layer.alert('登录已过期，请重新登录',
                { closeBtn: 0 },
                function () {
                    top.layer.closeAll();
                    top.loginout();
                });
        }
    },
    //&=转换成json
    parseJson(url) {
        try {
            var index = url.indexOf('?');
            url = url.match(/\?([^#]+)/)[1];
            var obj = {}, arr = url.split('&');
            for (var i = 0; i < arr.length; i++) {
                var subArr = arr[i].split('=');
                var key = decodeURIComponent(subArr[0]);
                var value = decodeURIComponent(subArr[1]);
                obj[key] = value;
            }
            return obj;

        } catch (err) {
            return null;
        }
    },
    //json 转 &=
    parseParams(data) {
        try {
            var tempArr = [];
            for (var i in data) {
                var key = encodeURIComponent(i);
                var value = encodeURIComponent(data[i]);
                tempArr.push(key + '=' + value);
            }
            var urlParamsStr = tempArr.join('&');
            return urlParamsStr;
        } catch (err) {
            return '';
        }
    },


    /**
 * 超标判断
 * @param {any} d 数据行
 * @param {any} xtag 因子
 * @param {any} tagname 因子
 */
    wartip: function (d, xtag, type) {
        if (d.a19001 && d.a01014 && d.a19001 > 19 && d.a01012 < 40) {
            return 0;
        }
        if (d[xtag + "_" + type] && (d[xtag + "_min2"] && d[xtag + "_" + type] < d[xtag + "_min2"])
            || (d[xtag + "_max2"] && d[xtag + "_" + type] > d[xtag + "_max2"])) {
            return 2;
        } else if (d[xtag + "_" + type] && (d[xtag + "_min1"] && d[xtag + "_" + type] < d[xtag + "_min1"])
            || (d[xtag + "_max1"] && d[xtag + "_" + type] > d[xtag + "_max1"])) {
            return 1;
        }
        else {
            return 0;
        }
    },

    /**
     * 超标预警
     * @param {any} d 数据行
     * @param {any} xtag 因子
     * @param {any} tagname 因子
     */
    warchaobiao: function (d, xtag, tagname, type) {
        if (d.a19001 && d.a01014 && d.a19001 > 19 && d.a01012 < 40) {
            return "<span>" + d[xtag + "_" + type] + "</span>";
        }

        if (d[xtag + "_" + type] && (d[xtag + "_min2"] && d[xtag + "_" + type] < d[xtag + "_min2"])
            || (d[xtag + "_max2"] && d[xtag + "_" + type] > d[xtag + "_max2"])) {
            return "<span class=\"red\" tips=\"超标报警，点击查看详情\" onclick='tipwar(" + d[xtag + "_min1"] + "," + d[xtag + "_min2"] + "," + d[xtag + "_max1"] + "," + d[xtag + "_max2"] + "," + d[xtag + "_" + type] + ",\"" + tagname + "\",\"" + d[xtag + "_xFlag"] + "\")'>" + d[xtag + "_" + type] + "</span>";
        } else if (d[xtag + "_" + type] && (d[xtag + "_min1"] && d[xtag + "_" + type] < d[xtag + "_min1"])
            || (d[xtag + "_max1"] && d[xtag + "_" + type] > d[xtag + "_max1"])) {
            return "<span class=\"yellow\"  tips=\"超标预警，点击查看详情\" onclick='tipwar(" + d[xtag + "_min1"] + "," + d[xtag + "_min2"] + "," + d[xtag + "_max1"] + "," + d[xtag + "_max2"] + "," + d[xtag + "_" + type] + ",\"" + tagname + "\",\"" + d[xtag + "_xFlag"] + "\")'>" + d[xtag + "_" + type] + "</span>";
        }
        else {
            if (d[xtag + "_" + type]) {
                return "<span>" + d[xtag + "_" + type] + "</span>";
            }
            else {
                return "--";
            }
        }
    },
    //恒定值
    warhengding: function (row, xtag, cnn, rows, type) {
        if (row.a19001 && row.a01014 && row.a19001 > 19 && row.a01012 < 40) {
            return false;
        }
        var index = row.LAY_INDEX;//当前行索引
        if (cnn == 2061) {//小时
            var dt0 = row.datatime;
            var timestamp = new Date(dt0.format("yyyy/MM/dd hh:mm:ss")).getTime();
            timestamp = timestamp - 1000 * 60 * 60 * 5;
            var end = new Date(timestamp).format("yyyyMMddhhmmss");
            var count = 0;
            for (var i = index + 1; i < rows.length; i++) {
                if (rows[i].datatime >= end) {
                    if (row[xtag + "_" + type] == rows[i][xtag + "_" + type]) {
                        count += 1;
                    } else {
                        return false;
                    }
                }
            }
            if (count >= 4) {
                return true;
            }
        }
        else if (cnn == 2031) {//日数据
            var dt0 = row.datatime;
            var timestamp = new Date(dt0.format("yyyy/MM/dd hh:mm:ss")).getTime();
            timestamp = timestamp - 1000 * 60 * 60 * 24 * 2;
            var end = new Date(timestamp).format("yyyyMMddhhmmss");
            var count = 0;
            for (var i = index + 1; i < rows.length; i++) {
                if (rows[i].datatime >= end) {
                    if (row[xtag + "_" + type] == rows[i][xtag + "_" + type]) {
                        count += 1;
                    } else {
                        return false;
                    }
                }
            }
            if (count >= 1) {
                return true;
            }
        }
        return false;
    },
    //缺失数据
    warNull: function (row, cnn, rows) {
        var index = row.LAY_INDEX;//当前行索引
        if (cnn == 2061) {//小时
            var dt0 = row.datatime;
            var timestamp = new Date(dt0.format("yyyy/MM/dd hh:mm:ss")).getTime();
            timestamp = timestamp - 1000 * 60 * 60 * 4;
            var end = new Date(timestamp).format("yyyyMMddhhmmss");
            var count = 0;
            for (var i = index; i < rows.length; i++) {
                if (rows[i].datatime >= end) {
                    count += 1;
                }
            }
            if (count == 1) {
                return true;
            }
        }
        else if (cnn == 2031) {//日数据
            var dt0 = row.datatime;
            var timestamp = new Date(dt0.format("yyyy/MM/dd hh:mm:ss")).getTime();
            timestamp = timestamp - 1000 * 60 * 60 * 24 * 3;
            var end = new Date(timestamp).format("yyyyMMddhhmmss");
            var count = 0;
            for (var i = index; i < rows.length; i++) {
                if (rows[i].datatime >= end) {
                    count += 1;
                }
            }
            if (count == 1) {
                return true;
            }
        }
        return false;
    },
    //氨氮大于总氮
    waradzd: function (row) {
        if (row["w21003_xRtd"] && row["w21001_xRtd"] && parseFloat(row["w21003_xRtd"]) > parseFloat(row["w21001_xRtd"])) {
            return true;
        } else {
            return false;
        }
    },

    /**
     * 数据预警
     * @param {any} row
     * @param {any} xtag
     * @param {any} tagname
     * @param {any} cnn
     * @param {any} rows
     * @param {any} value
     */
    wartips: function (row, xtag, tagname, cnn, rows, type) {
        if (row[xtag + "_" + type]) {
            var value = row[xtag + "_" + type];
            if ($.wartip(row, xtag, type) > 0) {
                return $.warchaobiao(row, xtag, tagname, type);
            } else if (value < 0 && xtag != "S08" && xtag != "a01006" && xtag !="a01013") {
                return "<span  tips='负值' class='red'>" + value + "</span>";
            } else if (value < 0.0001 && xtag != "S08" && xtag != "a01006" && xtag != "a01013") {
                return "<span  tips='0值' class='red'>" + value + "</span>";
            } else if (xtag == "w21003" && $.waradzd(row)) {
                return "<span class='red'  tips='氨氮浓度大于总氮浓度' >" + row["w21003_xRtd"] + "</span>";
            } else if ($.warhengding(row, xtag, cnn, rows, type)) {
                return "<span class='yellow' tips='浓度值长时间未变化' >" + value + "</span>";
            } else {
                return value;
            }
        }
        else {
            return "--";
        }

    },

    /**
     * 获取缺失数据的时间
     * @param {any} rows
     * @param {any} cnn
     * @param {any} start
     * @param {any} end
     */
    getnulldate: function (rows, cnn, start, end) {
        var startstamp = new Date(start).getTime();
        var endstamp = new Date(end).getTime();
        var interval = 1000 * 60 * 60;//小时
        if (cnn == 2031) {//日
            interval = 1000 * 60 * 60 * 24;
        }
        var arr = [];
        var curent = startstamp;
        while (curent < endstamp) {
            var hasdata = false;
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].datatime == new Date(curent).format("yyyyMMddhhmmss")) {//查找是否有时间段的数据
                    hasdata = true;
                    break;
                }
            }
            if (!hasdata) {
                arr.push(curent);
            }
            curent += interval;
        }
        return arr;
    }
});

//时间类型转字符串的函数
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
        (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
            RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

//2020011203245959
String.prototype.format = function (format) {
    var o = {
        "y+": this.substr(0, 4),
        "M+": this.substr(4, 2), //month
        "d+": this.substr(6, 2), //day
        "h+": this.substr(8, 2), //hour
        "m+": this.substr(10, 2), //minute
        "s+": this.substr(12, 2), //second
    }
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
            RegExp.$1.length >= 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}