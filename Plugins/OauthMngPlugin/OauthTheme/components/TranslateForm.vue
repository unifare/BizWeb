<template>
    <div id="translate-form">
        <form>
            <input type="text" v-model="textToTranslate" placeholder="输入需要翻译的内容">
            <select>
                <option value="en">English</option>
            </select>
            <input type="submit" value="翻译" v-on:click="formSubmit">
        </form>
    </div>
</template>

<script>
    export default {
        name: 'TranslateForm',
        data: function () {
            return {
                textToTranslate: '',
            }
        },
        methods: {
            formSubmit: function (e) {
                this.$emit('formSubmit', this.textToTranslate); //父组件监听的名字必须是formSubmit
                e.preventDefault();
            }
        }
    }
</script>

<style>
</style>