$(document).ready(function () {
    init();
    loadData();
})
function init() {
    $("#save").hide();
    $("#cancel").hide();
    $('#tensv').attr('readonly', true);
    $('#ngaysinh').attr('readonly', true);
    $('#lop').attr('disabled', true);
    $('#que').attr('readonly', true);
    $('#dantoc').attr('disabled', true);
}
//function date(day) {
//    var m = day.split('/')[0];
//    if (m.length == 1) m = "0" + m;
//    var d = day.split('/')[1];
//    if (d.length == 1) d = "0" + d;
//    var y = day.split('/')[2];
//    return y + "-" + m + "-" + d;
//}
function loadData() {
    $.ajax({
        type: "GET",
        url: "/SINHVIEN/ListAll",
        //data: { searchTerm: searchString },
        //contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (i, val) {
                html += '<tr id="row_' + val.ID + '"><td>' + val.TENSV + '</td>';
                html += '<td>' + val.LOP.TENLOP + '</td>';
                html += '<td>' + val.NGAYSINH + '</td>';
                html += '<td>' + val.QUEQUAN + '</td>';
                html += '<td>' + val.DANTOC.TENDT + '</td>';
                html += '<td><button onclick="Hienthi(' + parseInt(val.ID) + ')">Xem</button>';
                html += '<button onclick="Edit(' + parseInt(val.ID) + ')">Sửa</button>';
                html += '<button onclick="Delete(' + parseInt(val.ID) + ')">Xóa</button></td></tr>';
            });
            $('#tbody').html(html);
            Hienthi(data[0].ID);
        },
        error: function (errormessage) {
            alert(errormessage.responseJSON);
        }
    });
}
function Hienthi(ID) {
    $.ajax({
        type: "POST",
        url: "/SINHVIEN/Details",
        data: { id: ID },
        //contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#tensv").val(data.TENSV);
            $("#lop").val(data.MALOP);
            $("#ngaysinh").val(data.NGAYSINH);
            $("#que").val(data.QUEQUAN);
            $("#dantoc").val(data.MADANTOC);
            //$("#row_" + ID.toString()).toggleClass('highlight');
            $('#tbody tr').removeClass('highlight');
            $("#row_" + ID.toString()).addClass('highlight');
        },
        error: function (errormessage) {
            alert(errormessage.responseJSON);
        }
    });
}
function addSV() {
    $("#save").show();
    $("#cancel").show();
    $("#tensv").val("");
    $("#que").val("");
    $("#tensv").focus();
    $('#tensv').removeAttr('readonly', true);
    $('#ngaysinh').removeAttr('readonly', true);
    $('#lop').attr('disabled', false);
    $('#que').removeAttr('readonly', true);
    $('#dantoc').attr('disabled', false);
    $('#save').off('click').on('click', function () { Save(); });
    $('#cancel').off('click').on('click', function () { HuyAdd(); });
}
function Save() {
    var TENSV = $('#tensv').val();
    var NGAYSINH = $('#ngaysinh').val().toString();
    var QUEQUAN = $('#que').val();
    var MADANTOC = $('#dantoc').val();
    var MALOP = $('#lop').val();
    if (NGAYSINH != "") {
        $.ajax({
            type: "POST",
            url: "/SINHVIEN/Create",
            data: { ten: TENSV, malop: MALOP, ngaysinh: NGAYSINH, que: QUEQUAN, madantoc: MADANTOC },
            //contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (data) {
                //$('#save').unbind('click');
                //$('#cancel').unbind('click');
                var html = '';
                html += '<tr id="row_' + data.ID + '"><td>' + data.TENSV + '</td>';
                html += '<td>' + data.LOP.TENLOP + '</td>';
                html += '<td>' + data.NGAYSINH + '</td>';
                html += '<td>' + data.QUEQUAN + '</td>';
                html += '<td>' + data.DANTOC.TENDT + '</td>';
                html += '<td><button onclick="Hienthi(' + parseInt(data.ID) + ')">Xem</button>';
                html += '<button onclick="Edit(' + parseInt(data.ID) + ')">Sửa</button>';
                html += '<button onclick="Delete(' + parseInt(data.ID) + ')">Xóa</button></td></tr>';
                $('#tbody').append(html);
                init();
                Hienthi(data.ID);
            },
            error: function (errormessage) {
                alert(errormessage.responseJSON);
            }
        });
    }
    else {
        alert("Bạn phải nhập ngày sinh!");
    }
}
function HuyAdd() {
    loadData();
    init();
}
function Edit(ID) {
    Hienthi(ID);
    $("#save").show();
    $("#cancel").show();
    $("#tensv").focus();
    $('#tensv').removeAttr('readonly', true);
    $('#ngaysinh').removeAttr('readonly', true);
    $('#lop').attr('disabled', false);
    $('#que').removeAttr('readonly', true);
    $('#dantoc').attr('disabled', false);
    $('#save').off('click').on('click', function () { Update(ID); });
    $('#cancel').off('click').on('click', function () { HuyEdit(); });
}
function Update(ID) {
    var TENSV = $('#tensv').val();
    var NGAYSINH = $('#ngaysinh').val().toString();
    var QUEQUAN = $('#que').val();
    var MADANTOC = $('#dantoc').val();
    var MALOP = $('#lop').val();
    $.ajax({
        type: "POST",
        url: "/SINHVIEN/Edit",
        data: { id: ID, ten: TENSV, malop: MALOP, ngaysinh: NGAYSINH, que: QUEQUAN, madantoc: MADANTOC },
        //contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            //$('#save').unbind('click');
            //$('#cancel').unbind('click');
            var html = '';
            html += '<td>' + data.TENSV + '</td>';
            html += '<td>' + data.LOP.TENLOP + '</td>';
            html += '<td>' + data.NGAYSINH + '</td>';
            html += '<td>' + data.QUEQUAN + '</td>';
            html += '<td>' + data.DANTOC.TENDT + '</td>';
            html += '<td><button onclick="Hienthi(' + parseInt(data.ID) + ')">Xem</button>';
            html += '<button onclick="Edit(' + parseInt(data.ID) + ')">Sửa</button>';
            html += '<button onclick="Delete(' + parseInt(data.ID) + ')">Xóa</button></td>';
            $('#row_' + data.ID).html(html);
            init();
        },
        error: function (errormessage) {
            alert(errormessage.responseJSON);
        }
    });
}
function HuyEdit() {
    init();
}
function Delete(ID) {
    var result = confirm("Bạn có muốn xóa sinh viên này?");
    if (result) {
        $.ajax({
            type: "POST",
            url: "/SINHVIEN/Delete",
            data: { id: ID },
            //contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#row_' + data).remove();
            },
            error: function (errormessage) {
                alert(errormessage.responseJSON);
            }
        });
    }
}
function Search() {
    var searchString = $("#searchBox").val();
    $.ajax({
        type: "POST",
        url: "/SINHVIEN/Search",
        data: { searchString: searchString },
        //contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (i, val) {
                html += '<tr id="row_' + val.ID + '"><td>' + val.TENSV + '</td>';
                html += '<td>' + val.LOP.TENLOP + '</td>';
                html += '<td>' + val.NGAYSINH + '</td>';
                html += '<td>' + val.QUEQUAN + '</td>';
                html += '<td>' + val.DANTOC.TENDT + '</td>';
                html += '<td><button onclick="Hienthi(' + parseInt(val.ID) + ')">Xem</button>';
                html += '<button onclick="Edit(' + parseInt(val.ID) + ')">Sửa</button>';
                html += '<button onclick="Delete(' + parseInt(val.ID) + ')">Xóa</button></td></tr>';
            });
            $('#tbody').html(html);
            Hienthi(data[0].ID);
        },
        error: function (errormessage) {
            alert(errormessage.responseJSON);
        }
    });
}