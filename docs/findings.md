# Findings

## 2026-04-27 CLI 빌드/실행 검증 결과

## 문제 1: .NETFramework v4.6.1 참조 어셈블리 누락

- 증상:
  - `MSB3644: .NETFramework,Version=v4.6.1의 참조 어셈블리를 찾을 수 없습니다.`
- 원인:
  - 4.6.1 타기팅 팩 미설치 또는 불완전 설치
- 해결:
  - `.NET Framework 4.6.1 Developer Pack` 설치
- 확인:
  - `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1`에 DLL 존재
  - Release x86 빌드 성공

## 문제 2: HwpObjectLib COM 타입/클래스 등록 불일치

- 초기 증상(빌드 단계):
  - `TYPE_E_LIBNOTREGISTERED` 또는 `HwpObjectLib` 참조 실패
- 초기 증상(실행 단계):
  - `0x80040154 (REGDB_E_CLASSNOTREG)` with CLSID `{2291CF00-64A1-4877-A9B4-68CFE89612D6}`
- 해결 과정:
  - TypeLib 등록 확인: `{7D2B6F3C-1D95-4E0C-BF5A-5EE564186FBC}`
  - 관리자 권한으로 COM 등록 수행:
    - `regsvr32 HwpAutomation.dll`
    - `Hwp.exe /RegServer`
  - 최종 동작 확인:
    - `New-Object -ComObject HWPFrame.HwpObject` 성공
- 참고:
  - 32비트 COM 등록은 `WOW6432Node`에 나타날 수 있음

## 문제 3: ODT 변환 실패

- 재현:
  - `test.hwp` -> `--target ODT` 수행 시 실패 카운트 발생
- 상태:
  - 반복 재현됨
  - 원인 미확정 (추가 진단 필요)

## 포맷별 검증 결과 (test.hwp)

- 성공:
  - `PDF`, `HWPX`, `HWPML2X`, `HTML+`, `OOXML`, `UNICODE`, `RTF`
- 스킵:
  - `HWP` (입력과 동일 형식)
- 실패:
  - `ODT`

## 문제 4: old GUI는 설정 없이 동작했는데 현재는 COM 설정이 필요한 이유

검증 대상:

- old 바이너리 경로: `hwp2pdf-old-gui`
- 포함 파일:
  - `hwp2pdf.exe`
  - `AxInterop.HWPCONTROLLib.dll`
  - `Interop.HWPCONTROLLib.dll`
  - `FilePathCheckerModuleExample.dll`

분석 결과:

- old GUI 실행 파일은 `AxInterop.HWPCONTROLLib` 기반 `AxHwpCtrl`을 사용한다.
  - IL 덤프에서 `AxHwpCtrl::Open`, `AxHwpCtrl::SaveAs`, `AxHost/ClsidAttribute("bd9c32de-...")` 확인
- 현재 코드(소스)는 `HwpObjectLib` + `HWPFrame.HwpObject`를 직접 생성한다.
  - `CliRunner.cs`, `FormMain.cs`에서 `new HwpObject()` 사용
- 즉, old GUI와 현재 코드는 한글 자동화 경로가 동일하지 않다.

레지스트리 확인 포인트:

- `HWPFrame.HwpObject` 관련 CLSID는 32비트 뷰(`WOW6432Node`)에 존재할 수 있다.
- 실제 동작 판정은 레지스트리 단일 키보다 아래 테스트가 더 정확하다.

```powershell
try {
    $hwp = New-Object -ComObject HWPFrame.HwpObject
    "OK"
    $hwp.Quit()
} catch {
    "FAIL: $($_.Exception.Message)"
}
```

질문에 대한 결론:

- `AxInterop/Interop.HWPCONTROLLib.dll`은 COM 서버 본체가 아니라 인터롭 래퍼 DLL이다.
- 따라서 DLL만으로 현재 코드의 COM 의존(`HWPFrame.HwpObject`)을 완전히 제거할 수 없다.
- 다만 아래는 가능:
  - 빌드 시 COMReference 대신 파일 참조 기반으로 전환(개발 환경 의존성 감소)
  - 런타임에 COM 헬스체크 및 자동복구 가이드 제공(운영 편의 개선)
