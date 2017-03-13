<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VcCodeHtml.ascx.cs" Inherits="MyTest.VcCode.VcCodeHtml" %>
<script type="text/html" id="vcCodeHtmlSectionTmpl">
    <section class="vc-wrapper" style="display: block; height: 226px;">
        <div class="vc-btn-container" id="dragBtnContainer" style="display: block;">
            <div class="vc-slide-text" style="display: block;"><span>请按住滑块，拖动到最右边</span></div>
            <div class="vc-slideBtnLeft" id="dragBtnLeft" style="width: 0;">
                <span class="canvas-Title" style="display: none;">请点击图中的“<strong id="selectedChar"></strong>”字</span>
            </div>
            <div class="vc-slideBtn ui-draggable ui-draggable-handle" id="dragBtn" style="left: 0; top: 0px; float: left;"><i class="passport-icon ready-status"></i></div>
        </div>
        <div class="canvas-wrapper" style="">
            <div class="canvas-container" id="canvasContainer" style="height: 138px;">
                <img id="vcCanvas" class="vc-canvas" src="" alt="">
            </div>
            <div class="canvas-foot">
                <div class="fl">
                    <p class="tips" id="vcCodeTips"></p>
                </div>
                <div class="fr btn" style="display: none;" id="vcCodeRefresh">刷新</div>
            </div>
        </div>
        <div class="vc-close-btn"></div>
    </section>
</script>
