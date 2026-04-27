# Implementation

## 빌드 명령

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\MSBuild.exe" .\hwp2pdf.csproj /t:Rebuild /p:Configuration=Release /p:Platform=x86
```

## 사전 점검

1. .NET Framework 4.6.1 타기팅 팩 설치 여부 확인
2. COM 객체 생성 가능 여부 확인

```powershell
try {
    $hwp = New-Object -ComObject HWPFrame.HwpObject
    "OK: COM object created"
    $hwp.Quit()
} catch {
    "FAIL: $($_.Exception.Message)"
}
```

## 모든 포맷 일괄 변환

테스트 문서 `test.hwp`를 지원 대상 전체 포맷으로 변환하는 명령:

```powershell
$exe = ".\bin\x86\Release\hwp2pdf.exe"
$input = ".\test.hwp"
$targets = @("PDF","HWP","HWPX","HWPML2X","HTML+","ODT","OOXML","UNICODE","RTF")

foreach ($t in $targets) {
    $out = ".\out-all-formats\$($t.Replace('+','plus'))"
    New-Item -ItemType Directory -Force -Path $out | Out-Null
    & $exe $input --output $out --target $t --overwrite overwrite
    "target=$t exit=$LASTEXITCODE"
}
```

참고:

- `HWP` 타겟은 입력이 이미 `.hwp`일 경우 같은 형식으로 판단되어 스킵된다.
- `HTML+`는 `.html` 외에 css/png 리소스 파일이 함께 생성된다.

## 이번 검증 결과 (2026-04-27)

- 성공: `PDF`, `HWPX`, `HWPML2X`, `HTML+`, `OOXML`, `UNICODE`, `RTF`
- 스킵: `HWP` (같은 형식)
- 실패: `ODT` (원인 미확정, 재현됨)

## old GUI DLL 재사용 관련 가이드

질문:

- `hwp2pdf-old-gui`의 `AxInterop.HWPCONTROLLib.dll`, `Interop.HWPCONTROLLib.dll`을 활용하면 설정 없이 동작할 수 있는가?

결론:

- 빌드 측면:
  - 가능성이 있음. `COMReference` 대신 파일 참조 방식으로 전환하면 개발 PC에서 COM 타입 라이브러리 미등록 상태에서도 빌드는 가능해질 수 있다.
- 런타임 측면:
  - 완전 무설정은 어려움. 인터롭 DLL은 COM 서버 본체가 아니라 .NET 래퍼이므로, 실제 한글 COM 서버 등록이 없으면 `Class not registered`가 발생할 수 있다.

권장 운영 방식:

1. 실행 시작 시 `New-Object -ComObject HWPFrame.HwpObject` 헬스체크
2. 실패 시 관리자 권한으로 COM 등록 안내
3. 등록 후 재시도
