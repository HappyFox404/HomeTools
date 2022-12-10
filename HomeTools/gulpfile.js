/// <binding BeforeBuild='build-js' ProjectOpened='watch' />
"use strict";

const jsLibs = [
    './wwwroot/node_modules/jquery/dist/jquery.min.js',
    './wwwroot/node_modules/chart.js/dist/chart.umd.js'
];

const sass = require('gulp-sass')(require('sass'));
let gulp = require("gulp"),
    rename = require("gulp-rename"),
    csso = require("gulp-csso"),
    uglify = require("gulp-uglify"),
    concat = require("gulp-concat");

var paths = {
    styles: "./wwwroot/sass/main.scss",
    production: './wwwroot/dist',
    scriptsSrc: "./wwwroot/js",
    stylesSrc: "./wwwroot/sass"
};

gulp.task("build-style", function () {
    return gulp.src(paths.styles)
        .pipe(sass())
        .pipe(csso())
        .pipe(rename({ basename:'production', suffix: '.min' }))
        .pipe(gulp.dest(paths.production))
});

gulp.task("build-libs", function () {
    return gulp.src(jsLibs)
        .pipe(concat("libs.js"))
        //.pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(paths.production));
});

gulp.task("build-scripts", function () {
    return gulp.src([paths.scriptsSrc + "/routes.js", paths.scriptsSrc + "/common.js", paths.scriptsSrc + "/**/*.js"])
        .pipe(concat("production.js"))
        //.pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(paths.production));
});

gulp.task("build-min", gulp.series(
    gulp.parallel(
        'build-style',
        'build-scripts'
    )
));
gulp.task("build-full", gulp.series(
    gulp.parallel(
        'build-style',
        'build-scripts',
        'build-libs'
    )
));


gulp.task('watch', function () {
    gulp.watch(paths.stylesSrc + '**/*.scss', gulp.series('build-style'));
    gulp.watch(paths.scriptsSrc + '**/*.js', gulp.series('build-scripts'));
});