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
