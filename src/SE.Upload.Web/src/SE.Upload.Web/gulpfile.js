/// <binding Clean='clean' />

var gulp = require("gulp"),
  rimraf = require("gulp-rimraf"),
 include = require("gulp-include"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-minify-css"),
   gutil = require('gulp-util'),
  uglify = require("gulp-uglify"),
  rename = require("gulp-rename"),
 replace = require("gulp-replace"),
    less = require("gulp-less"),
     tsc = require('gulp-typescript'),
   debug = require('gulp-debug'),
sequence = require('run-sequence');
 project = require("./project.json");
tsConfig = require("./tsconfig.json");

var webroot = "./" + project.webroot + "/";

var js = {
           srcTs: webroot + "scripts/ts/**/*.ts",
     outFolderTs: webroot + "scripts/ts/",
      outFilesTs: webroot + "scripts/ts/**/*.js",
       typingsTs: webroot + "scripts/ts/typings/**/*.d.ts",
      srcBundles: webroot + "scripts/*.bundle",
outFolderBundles: webroot + "scripts/",
 outFilesBundles: webroot + "scripts/*.js",
          srcMin: webroot + "scripts/*.js",
    outFolderMin: webroot + "scripts/",
     outFilesMin: webroot + "scripts/*.min.js"
};

var css = {
         srcLess: webroot + "styles/less/**/*.less",
         incLess: webroot + "styles/less/**/_*.less",
   outFolderLess: webroot + "styles/less",
    outFilesLess: webroot + "styles/less/**/*.css",
      srcBundles: webroot + "styles/*.bundle",
outFolderBundles: webroot + "styles/",
 outFilesBundles: webroot + "styles/*.css",
          srcMin: webroot + "styles/*.css",
    outFolderMin: webroot + "styles/",
     outFilesMin: webroot + "styles/*.min.css"
};

gulp.task("ts:clean", function () {
    return gulp.src([js.outFilesTs])
        .pipe(debug({ title: "delete:" }))
        .pipe(rimraf());
});

gulp.task("ts:build", function () {
    return gulp.src([js.srcTs, "!" + js.typingsTs])
        .pipe(debug({ title: "build:" }))
        .pipe(tsc(tsConfig.compilerOptions))
        .pipe(gulp.dest(js.outFolderTs));
});

gulp.task("js:clean", function () {
    return gulp.src([js.outFilesBundles])
        .pipe(debug({ title: "delete:" }))
        .pipe(rimraf());
});

gulp.task("js:bundle", function () {
    return gulp.src(js.srcBundles)
        .pipe(debug({ title: "bundle:" }))
		.pipe(include())
		.pipe(rename({
		    extname: ".js"
		}))
		.pipe(gulp.dest(js.outFolderBundles));
});

gulp.task("js:min", function () {
    return gulp.src([js.srcMin, "!" + js.outFilesMin])
        .pipe(debug({ title: "min:" }))
        .pipe(uglify())
        .pipe(rename({
            extname: ".min.js"
        }))
        .pipe(gulp.dest(js.outFolderMin));
});

gulp.task("scripts:clean", ["ts:clean", "js:clean"]);

gulp.task("scripts:rebuild", function (cb) {
    sequence("scripts:clean", "ts:build", "js:bundle", "js:min", cb);
});

gulp.task("less:clean", function () {
    return gulp.src([css.outFilesLess])
        .pipe(debug({ title: "delete:" }))
        .pipe(rimraf());
});

gulp.task("less:build", function () {
    return gulp.src([css.srcLess, "!" + css.incLess])
        .pipe(debug({ title: "build:" }))
		.pipe(less())
		.pipe(gulp.dest(css.outFolderLess));
});

gulp.task("css:clean", function () {
    return gulp.src([css.outFilesBundles])
        .pipe(debug({ title: "delete:" }))
        .pipe(rimraf());
});

gulp.task("css:bundle", function () {
    return gulp.src(css.srcBundles)
        .pipe(debug({ title: "bundle:" }))
		.pipe(include())
		.pipe(rename({
		    extname: ".css"
		}))
		.pipe(gulp.dest(css.outFolderBundles));
});

gulp.task("css:min", function () {
    gulp.src([css.srcMin, "!" + css.outFilesMin])
        .pipe(debug({ title: "min:" }))
        .pipe(cssmin())
		.pipe(replace(/(\.\.\/){2,}/g, "../"))
        .pipe(rename({
            extname: ".min.css"
        }))
        .pipe(gulp.dest(css.outFolderMin));
});

gulp.task("styles:clean", ["less:clean", "css:clean"]);

gulp.task("styles:rebuild", function (cb) {
    sequence("styles:clean", "less:build", "css:bundle", "css:min", cb);
});

gulp.task("dev:less", function () {
    gulp.watch(css.srcLess, ["less:build"]);
});

gulp.task("dev:ts", function () {
    gulp.watch(js.srcTs, ["ts:build"]);
});

gulp.task("dev", ["dev:less", "dev:ts"]);