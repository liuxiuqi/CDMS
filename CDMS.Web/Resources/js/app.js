layui.define(['element', 'tab', 'navbar', 'onelevel', 'utils'], function (exports) {
    var $ = layui.jquery,
        element = layui.element,
        layer = layui.layer,
        navbar = layui.navbar,
        tab = layui.tab,
        utils = layui.utils;

    var version = layui.cache.version;
    var app = {
        config: {
            type: 'iframe'
        },
        set: function (options) {
            var that = this;
            $.extend(true, that.config, options);
            return that;
        },
        init: function () {
            var that = this, _config = that.config;
            if (_config.type === 'iframe') {
                tab.set({
                    mainUrl: '/main/home',
                    elem: '#container',
                    onSwitch: function (data) {
                        //选项卡切换时触发
                        //console.log(data.layId); //lay-id值
                        //console.log(data.index); //得到当前Tab的所在下标
                        //console.log(data.elem); //得到当前的Tab大容器
                    }
                }).render();

                navbar.set({
                    remote: {
                        url: '/sys/menu/getAuthMenuList?v=' + version,
                        type: 'post'
                    }
                }).render(function (data) {
                    tab.tabAdd(data);
                });

                //处理顶部一级菜单
                var onelevel = layui.onelevel;
                if (onelevel.hasElem()) {
                    onelevel.set({
                        remote: {
                            url: '/datas/onelevel1.json' //远程地址
                        },
                        onClicked: function (id) {
                            switch (id) {
                                case 1:
                                    navbar.set({
                                        remote: {
                                            url: '/datas/navbar1.json'
                                        }
                                    }).render(function (data) {
                                        tab.tabAdd(data);
                                    });
                                    break;
                                case 2:
                                    navbar.set({
                                        remote: {
                                            url: '/datas/navbar2.json'
                                        }
                                    }).render(function (data) {
                                        tab.tabAdd(data);
                                    });
                                    break;
                                default:
                                    navbar.set({
                                        data: [{
                                            id: "1",
                                            title: "基本元素",
                                            icon: "fa-cubes",
                                            spread: true,
                                            children: [{
                                                id: "7",
                                                title: "表格",
                                                icon: "&#xe6c6;",
                                                url: "test.html"
                                            }, {
                                                id: "8",
                                                title: "表单",
                                                icon: "&#xe63c;",
                                                url: "form.html"
                                            }]
                                        }, {
                                            id: "5",
                                            title: "这是一级导航",
                                            icon: "fa-stop-circle",
                                            url: "https://www.baidu.com",
                                            spread: false
                                        }]
                                    }).render(function (data) {
                                        tab.tabAdd(data);
                                    });
                                    break;
                            }
                        },
                        renderAfter: function (elem) {
                            elem.find('li').eq(0).click(); //模拟点击第一个
                        }
                    }).render();
                }
            }
            var addRippleEffect = function (e) {
                layui.stope(e)
                var target = e.target;
                if (target.localName !== 'button' && target.localName !== 'a') return false;
                var rect = target.getBoundingClientRect();
                var ripple = target.querySelector('.ripple');
                if (!ripple) {
                    ripple = document.createElement('span');
                    ripple.className = 'ripple'
                    ripple.style.height = ripple.style.width = Math.max(rect.width, rect.height) + 'px'
                    target.appendChild(ripple);
                }
                ripple.classList.remove('show');
                var top = e.pageY - rect.top - ripple.offsetHeight / 2 - document.body.scrollTop;
                var left = e.pageX - rect.left - ripple.offsetWidth / 2 - document.body.scrollLeft;
                ripple.style.top = top + 'px'
                ripple.style.left = left + 'px'
                ripple.classList.add('show');
                return false;
            }
            document.addEventListener('click', addRippleEffect, false);

            $('#logouta').on('click', function () {
                app.logout();
            });

            $('dl.skin > dd').on('click', function () {
                var $that = $(this);
                var skin = $that.children('a').data('skin');
                utils.skin.switchSkin(skin);
            });
            return that;
        },
        //skin: {
        //    setSkin: function (value) {
        //        layui.data('kit_skin', {
        //            key: 'skin',
        //            value: value
        //        });
        //    },
        //    getSkinName: function () {
        //        return layui.data('kit_skin').skin;
        //    },
        //    switchSkin: function (value) {
        //        var _target = $('link[kit-skin]')[0];
        //        var href = _target.href;
        //        _target.href = href.substring(0, href.lastIndexOf('/') + 1) + value + href.substring(href.lastIndexOf('.')) + '?v=' + version;
        //        this.setSkin(value);
        //    },
        //    init: function () {
        //        var skin = this.getSkinName();
        //        this.switchSkin(skin || 'default');
        //    }
        //},
        logout: function () {
            utils.confirm('注:您确定要安全退出本次登录吗?', function () {
                window.setTimeout(function () {
                    utils.ajax('/main/logout', {}, function (res) {
                        var flag = res.Type <= 1;
                        if (flag) window.location.href = "/";
                        else utils.alert(res.Msg);
                    }, false);
                }, 300);
            });
        }
    };

    //输出test接口
    exports('app', app);
});