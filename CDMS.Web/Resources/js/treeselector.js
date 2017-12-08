
layui.define(['jquery', 'utils'], function (exports) {
    var moduleName = 'treeSelector';
    var utils = layui.utils, $ = layui.jquery;
    var loadType = {
        local: 'local',
        remote: 'remote'
    };

    function TreeSelector() {
        var that = this;
        that.config = {
            rootId: 0,
            type: loadType.remote,
            selector: '',
            url: '',
            data: [],
            preString: '┠',
            repeatString: '--',
            dataOption: {
                idname: 'id',
                pidname: 'pid',
                textname: 'text',
                valuename: 'value'
            },
            callback: null
        };
        that.version = '1.2';
    }

    TreeSelector.prototype.set = function (options) {
        var that = this;
        $.extend(true, that.config, options);
        return that;
    }

    TreeSelector.prototype.render = function () {
        var that = this;
        var config = that.config;
        if (config.type == loadType.local) {
            loadTree(config);
        }
        else {
            utils.post(config.url, {}, function (data) {
                config.data = data;
                loadTree(config);
            }, false);
        }
    }

    function loadTree(config) {
        var data = config.data;
        if (!data || data.length < 1) return;
        var dom = $(config.selector);
        var option = config.dataOption;
        layui.each(data, function (i, item) {
            if (item[option.pidname] == config.rootId) {
                createTree(1, item, config, dom);
            }
        });
        if (config.callback) {
            config.callback(config);
        }
    }

    function createTree(level, item, config, dom) {
        var data = config.data;
        var option = config.dataOption;
        var blank = '', arr = [];
        if (level > 1) {
            for (var i = 0; i < level; i++) {
                blank += config.repeatString;
            }
            blank += config.preString;
        }

        dom.append('<option value="' + item[option.valuename] + '">' + blank + item[option.textname] + '</option>');// 添加Option选项  

        layui.each(data, function (i, obj) {
            if (obj[option.pidname] == item[option.idname]) {
                createTree(level + 1, obj, config, dom)
            }
        });
    }

    var tree = new TreeSelector();

    exports(moduleName, tree);
});