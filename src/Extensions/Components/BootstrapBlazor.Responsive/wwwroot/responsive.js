// 保存组件ID及dotnet引用
let lastBreakpoint = null;
const components = [];
const onBreakpoint = function (dotnetInterop, currentBreakpoint) {
    dotnetInterop.invokeMethodAsync('OnBreakpoint', currentBreakpoint);
};
const onResized = function () {
    if (components && components.length > 0) {
        var currentBreakpoint = getBreakpoint();

        if (lastBreakpoint !== currentBreakpoint) {
            lastBreakpoint = currentBreakpoint;

            let index = 0;

            for (index = 0; index < components.length; ++index) {
                onBreakpoint(components[index].dotnetInterop, currentBreakpoint);
            }
        }
    }
};

// 调整大小时重新计算断点
if (window.attachEvent) {
    window.attachEvent('onresize', onResized);
}
else if (window.addEventListener) {
    window.addEventListener('resize', onResized, true);
}
else {
    //浏览器不支持Javascript事件绑定
}


export function getBreakpoint() {
    return window.getComputedStyle(document.body, ':before').content.replace(/\"/g, '');
}

export function addBreakpointComponent(elementId, dotnetInterop) {
    components.push({ elementId: elementId, dotnetInterop: dotnetInterop });
}

export function findBreakpointComponentIndex(elementId) {
    let index = 0;

    for (index = 0; index < components.length; ++index) {
        if (components[index].elementId === elementId)
            return index;
    }

    return -1;
}

export function isBreakpointComponent(elementId) {
    let index = 0;

    for (index = 0; index < components.length; ++index) {
        if (components[index].elementId === elementId)
            return true;
    }

    return false;
}

export function registerBreakpointComponent(dotnetInterop, elementId) {
    if (lastBreakpoint == null) {
        // 初始化
        lastBreakpoint = getBreakpoint();
        onBreakpoint(dotnetInterop, lastBreakpoint);
    }

    if (isBreakpointComponent(elementId) !== true) {
        addBreakpointComponent(elementId, dotnetInterop);
    }
}

export function unregisterBreakpointComponent(elementId) {
    const index = findBreakpointComponentIndex(elementId);
    if (index !== -1) {
        components.splice(index, 1);
    }
}
