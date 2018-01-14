$.ajaxSetup({
    global: true,
    cache: false
});// 不要cache

$(".s_main,.sidetool").css("min-height",$(".service_bd").height());
$(".s_side").css("min-height", $(".s_side").height());

 function setheight() {
     var _body_h=$("body").height();
     var all_h=$(window).height();
     var cd_h=$(document).height();
     $(".structtable").height(all_h);
     $(".st-side").css("min-height",cd_h);
     $(".gateway_bd .list").css("min-height",all_h);
     $(".comm").css("min-height",cd_h-200);
     $(".monitor-bd .list").css("min-height",_body_h);
 }

$(document).ready(function () {


    var _body_h=$("body").height();
    var all_h=$(window).height();
    var cd_h=$(document).height();
    $(".structtable").height(all_h);
    $(".st-side ").css("min-height",cd_h-50);
    $(".comm,.service_bd ,.service_bd .s_side").css("min-height",cd_h-200);
    $(".gateway_bd .list").css("min-height",all_h);
    $(".monitor-bd .list").css("min-height",_body_h);

    var _bigh = $(".all").height();
    $(".leftcat,.stp3").css('min-height',cd_h);

    $("[clg]").on("click", function () {
        var _v = $(this).attr("clg");
        $("[toolg='" + _v + "']").toggle();
    })

    $("a[channav]").on("click", function () {
        var siz = $(this).attr("channav");
        $("[changbd]").hide();
        $("[channav]").removeClass("active");
        $(this).addClass("active");
        $(".changbd" + siz).show();

    })

    $(document).click(function () {
        $("[toolg]").hide();
    });
	
	
    $("[clg]").click(function (e) {
        e ? e.stopPropagation() : event.cancelBubble = true;
    });

});

function showld(d) {
    $(".acopying").html(d);
}
function overld() {
    setTimeout(delay, 500);

    function delay() {
        $(".acopying").html("");
    }
}


$("[changepassword]").on("click",function () {
   var _i= $(this).attr("changepassword");
   if(_i==0){
        $(this).text('隐藏').attr("changepassword",'1').removeClass('n').siblings('input').attr('type',"text");
   }
    if(_i==1){
        $(this).text('显示').attr("changepassword",'0').addClass('n').siblings('input').attr('type',"password");
    }
})

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

$("#gobt").on("click", function () {
    $(this).addClass("disabled").html("正在保存...");
})


$("input[qx]").on("click", function () {
    $(".ckbox").find("label").removeClass("on");
    $(this).parent("label").addClass("on");
})


function deleteData(msg, table, id) {
    if (window.confirm(msg)) {
        var url = easyapi.ctx + "/" + table + "!delete.action?id=" + id;
        $.get(url, function (data) {
            $('#' + table + id).slideUp("slow");
        });
        return true;
    }
    return false;
}

function state(msg, table, id, state) {
    if (window.confirm(msg)) {
        var url = easyapi.ctx + "/" + table + "!state.action?id=" + id + "&" + table + ".state=" + state;
        $.get(url, function (data) {
            $('#' + table + id).slideUp("slow");
        });
        return true;
    }
    return false;
}


//用户的启用禁用
function userEnabled(msg, table, id) {
    if (window.confirm(msg)) {
        var url = easyapi.ctx + "/" + table + "!userEnabled.action?id=" + id;
        $.get(url, function (data) {
            $('#' + table + id).slideUp("slow");
        });
        return true;
    }
    return false;
}

//获取短信对按钮的操作
function sms(time, btn) {
    var handle;//事件柄
    $("#" + btn).attr("disabled", true);
    handle = setInterval(function () {
        if (time > 0) {
            time = time - 1;
            $("#" + btn).val("已发送至邮箱" + time);
        } else {
            $("#" + btn).val("获取验证码");
            $("#" + btn).attr("disabled", false);
            clearInterval(handle);
        }
    }, 1000);
}

function lodingbg(time) {
    var _load = '<div class="spinner"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>';
    $("body").append(_load + "<div class='bgl' style='position:fixed;top:0;right:0;bottom: 0;left:0;z-index:1040;background: #000;opacity:.6;'></div>");
	if(time){
			setTimeout("closeloding()", time);
		}
}
function closeloding() {
    $(".spinner,.bgl").remove();
}


$("[data-href]").on("click",function () {
   var _url= $(this).attr("data-href");
   window.location.href=_url;
})

 $("div [id^='themcc_']").each(function (i, ob) {
                $(ob).find('span').bind('click', function () {
                    $(ob).find("span").removeClass('active').eq($(this).index()).addClass("active");
                    $(ob).siblings('div').hide().eq($(this).index()).show(); 
            });
			
 })


 
$("body").on("click",".layui-layer-close",function () {
    layer.closeAll();
})